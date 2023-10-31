using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using Microsoft.AspNetCore.Mvc.Rendering;
using CYRetailIMS.Application.ExternalService.ItemTransferAPI;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer;
using OfficeOpenXml;
using CYRetailIMS.Application.Common.Confiuration;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
using Microsoft.CodeAnalysis.Operations;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.Stock)]
public class ItemController : BaseController
{
    private readonly IWebHostEnvironment _env;
    private readonly IAppConfig _appConfig;
    private readonly IItemAPI _itemAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemTypeAPI _itemTypeAPI;
    private readonly IItemUnitOfMeasureAPI _itemUnitOfMeasureAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IItemTransferAPI _itemTransferAPI;
    private readonly IItemInBranchAPI _itemInBranchAPI;

    public ItemController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IAppConfig appConfig,
        IWebHostEnvironment webHostEnvironment,
        IItemAPI itemAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTypeAPI itemTypeAPI,
        IItemUnitOfMeasureAPI itemUnitOfMeasureAPI,
        IBranchAPI branchAPI,
        IItemTransferAPI itemTransferAPI,
        IItemInBranchAPI itemInBranchAPI) : base(httpClientRequest, mapper, log)
    {
        _env = webHostEnvironment;
        _appConfig = appConfig;
        _itemAPI = itemAPI;
        _itemTypeAPI = itemTypeAPI;
        _itemBrandAPI = itemBrandAPI;
        _itemUnitOfMeasureAPI = itemUnitOfMeasureAPI;
        _branchAPI = branchAPI;
        _itemTransferAPI = itemTransferAPI;
        _itemInBranchAPI = itemInBranchAPI;
    }

    public async Task<IActionResult> Index()
    {
        BaseResponse<List<GetBranchResponseDTO>> resBranchList = null;
        if(base.UserProfile.roleid == (int)EnumModel.UserRole.Admin)
        {
            resBranchList = await _branchAPI.GetBranchListAsync();
        }
        else
        {
            BaseResponse<GetBranchResponseDTO> resUserBranch = await _branchAPI.GetBranchByIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);
            if (resUserBranch.result)
            {
                resBranchList = new BaseResponse<List<GetBranchResponseDTO>>
                {
                    result = resUserBranch.result,
                    data = new List<GetBranchResponseDTO>
                    {
                        resUserBranch.data
                    },
                    error = resUserBranch.error,
                    message = resUserBranch.message,
                    soruce = resUserBranch.soruce,
                    status = resUserBranch.status
                };
            }
        }
        ViewBag.BranchList = resBranchList;
        return View();
    }

    [CustomAuthorize(RoleName.Admin, RoleName.Stock)]
    public async Task<IActionResult> Create()
    {
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();
        BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _itemUnitOfMeasureAPI.GetUnitOfMeasureListAsync();
        ViewBag.ItemTypeList = resItemTypeList;
        ViewBag.ItemBrandList = resItemBrandList;
        ViewBag.ItemUOMList = resUnitOfMeasureList;
        return View();
    }

    public IActionResult Import()
    {
        return View();
    }

    public IActionResult Adjust()
    {
        return View();
    }

    public async Task<IActionResult> Transfer()
    {
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await _itemAPI.GetItemListAsync();
        ViewBag.ItemList = resItemList;
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ItemTransferList = await GetItemTransferItemListByTransferType((int)TransferType.WTB);
        ViewBag.ItemTransaferTypeList = await PrepareSelectItemTransferType(); ;
        return View();
    }

    public async Task<IActionResult> TransferHistory()
    {
        BaseResponse<List<GetItemTransferResponseDTO>> transferHistory = null;
        if (base.UserProfile.roleid == (int)UserRole.Admin)
        {
            transferHistory = await _itemTransferAPI.GetItemTransferListAsync();
        }
        else
        {
            transferHistory = await _itemTransferAPI.GetItemTransferByDestinationBranchIDAsync(new GetItemTransferByDestinationBranchIDQuery
            {
                destinationbranchid = base.UserProfile.access_branch.FirstOrDefault().branchid
            });
        }

        ViewBag.ItemTransferHistory = transferHistory;
        ViewBag.ItemTransferStatus = await _itemTransferAPI.GetItemTransferStatusAsync();
        return View();
    }

    public async Task<IActionResult> ReceiveItemTransfer(int transferid)
    {
        BaseResponse<GetItemTransferResponseDTO> resTransferData = await _itemTransferAPI.GetItemTransferByIDAsync(transferid);
        ReceiveTransferItemViewModel viewModel = ReceiveTransferMapping(resTransferData.data);

        BaseResponse<List<GetBranchResponseDTO>> resBrachList = await _branchAPI.GetBranchListAsync();
        BaseResponse<GetItemListResponseDTO> resItem = await _itemAPI.GetItemByIdAsync(resTransferData.data.itemid);

        ViewBag.TransferTypeList = await _itemTransferAPI.GetItemTransferTypeAsync();
        ViewBag.TransferStatusList = await _itemTransferAPI.GetItemTransferStatusAsync();
        ViewBag.SourceBranchList = resBrachList;
        ViewBag.DestinationBranchList = resBrachList;
        ViewBag.TransferItem = resItem;
        return View(viewModel);
    }

    public IActionResult Detail(int itemid)
    {
        return View();
    }

    public async Task<IActionResult> Edit(int itemid)
    {
        //Get Item Detail
        BaseResponse<GetItemListResponseDTO> resItem = await _itemAPI.GetItemByIdAsync(itemid);
        EditItemViewModel viewModel = EditItemMapping(resItem.data);

        //Get Master Data
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();
        BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _itemUnitOfMeasureAPI.GetUnitOfMeasureListAsync();

        ViewBag.ItemTypeList = resItemTypeList;
        ViewBag.ItemBrandList = resItemBrandList;
        ViewBag.ItemUOMList = resUnitOfMeasureList;
        return View(viewModel);
    }

    public async Task<IActionResult> EditItemBranch(int itemid, int branchid)
    {
        //Get Item Detail
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItem = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(branchid);
        GetItemInBranchByBranchIDItemResponseDTO itemBranch = resItem.data.itemlist.FirstOrDefault(w => w.itemid == itemid);
        EditItemViewModel viewModel = EditItemInBranchMapping(itemBranch);
        viewModel.BranchID = branchid;

        //Get Master Data
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();
        BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _itemUnitOfMeasureAPI.GetUnitOfMeasureListAsync();

        ViewBag.ItemTypeList = resItemTypeList;
        ViewBag.ItemBrandList = resItemBrandList;
        ViewBag.ItemUOMList = resUnitOfMeasureList;
        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> ItemDataValidation(TransferItemViewModel transferItemObj)
    {
        try
        {
            #region Get form value
            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> form = Request.Form.ToList();
            #endregion

            #region Prepare new from with not empty value
            form = form.Where(w => w.Key.Contains("data[outer-item-group]")).Where(w => !string.IsNullOrEmpty(w.Value[0])).ToList();
            if (form.Count == 0)
            {
                return Json(new { result = false, msg = $"ขออภัย ข้อมูลขายสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            #region Validate Selling Item
            bool isValidData = form.Where(w => w.Key.Contains("data[outer-item-group]")).Any(w => !string.IsNullOrEmpty(w.Value[0]));
            if (!isValidData)
            {
                return Json(new { result = false, msg = $"ขออภัย ข้อมูลขายสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            return Json(new { result = true, msg = "ตรวจสอบข้อมูลถูกต้อง." });
        }
        catch (Exception ex)
        {
            return Json(new { result = true, msg = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveTransaferItem(TransferItemViewModel transferItemObj)
    {
        try
        {
            //if (!base.UserProfile.access_branch.Any(w => w.branchid == transferItemObj.source_branchid.ToInt32()))
            //{
            //    return Json(new { result = false, msg = $"{GlobalMessageModel.ErrorInvalidBranch}" });
            //}

            #region Get form value
            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> form = Request.Form.ToList();
            #endregion

            #region Prepare new from with not empty value
            form = form.Where(w => w.Key.Contains("outer-item-group")).Where(w => !string.IsNullOrEmpty(w.Value[0])).ToList();
            if (form.Count == 0)
            {
                return Json(new { result = false, msg = $"ขออภัย ข้อมูลขายสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            #region PrePare ItemTransfer List
            List<CreateItemTransferDetailCommand> itemTransferList = new List<CreateItemTransferDetailCommand>();
            #endregion

            decimal totalAmt = 0;
            decimal totalProfitAmt = 0;
            int idx = form.Count / 4;
            for (int i = 0; i < idx; i++)
            {
                var itemid = form.Where(w => w.Key == $"outer-item-group[{i}][ddlSearchItem]").FirstOrDefault().Value[0];
                var itemprice = form.Where(w => w.Key == $"outer-item-group[{i}][txtItemPrice]").FirstOrDefault().Value[0];
                var currentQty = form.Where(w => w.Key == $"outer-item-group[{i}][txtCurrentQty]").FirstOrDefault().Value[0];
                var transferQty = form.Where(w => w.Key == $"outer-item-group[{i}][txtTransferQty]").FirstOrDefault().Value[0];

                if (!string.IsNullOrEmpty(itemid) &&
                    !string.IsNullOrEmpty(itemprice) &&
                    !string.IsNullOrEmpty(currentQty) &&
                    !string.IsNullOrEmpty(transferQty))
                {
                    itemTransferList.Add(new CreateItemTransferDetailCommand
                    {
                        itemid = itemid.ToInt32(),
                        qty = transferQty.ToInt32()
                    });
                }
            }

            #region Prepare & Create Transaction
            CreateItemTransferCommand createItemTransferCommand = CreateItemTransferCommand(transferItemObj, itemTransferList);
            BaseResponse<CommandResponse> resCreateTrn = await _itemTransferAPI.CreateItemTransferAsync(createItemTransferCommand);
            if (!resCreateTrn.result)
            {
                return Json(new { result = false, msg = resCreateTrn.error.error.message });
            }
            #endregion
            return Json(new { result = true, msg = "บันทึกข้อมูลสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย รูปแบบข้อมูลไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetItemByID(string itemId, int transfertypeID, int sourceBranchID)
    {
        try
        {
            if (transfertypeID == (int)TransferType.WTB)
            {
                //คลัง-สาขา get from TMItem
                BaseResponse<GetItemListResponseDTO> resItem = await _itemAPI.GetItemByIdAsync(Convert.ToInt32(itemId));
                if (resItem.result)
                {
                    return Json(new { result = true, data = resItem.data, msg = "สำเร็จ" });
                }
                return Json(new { result = false, msg = resItem.error.error.message });
            }
            else
            {
                //สาขา-สาขา get from TMItemInBranch
                BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(sourceBranchID);
                var resFilterItem = resItemInBranch.data.itemlist.FirstOrDefault(w => w.itemid == itemId.ToInt32());
                if (resFilterItem != null)
                {
                    return Json(new { result = true, data = resFilterItem, msg = "สำเร็จ" });
                }
                return Json(new { result = false, msg = "ไม่พบข้อมูลสินค้า" });
            }

        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย, ไม่พบข้อมูลสินค้า. <br> {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] AddItemViewModel addItemObj)
    {
        CreateItemCommand createItemCommand = MappingCreateItemCommand(addItemObj);
        BaseResponse<CommandResponse> resCreateItem = await _itemAPI.CreateItemAsync(createItemCommand);
        if (resCreateItem.result)
        {
            return Json(new JsonViewModel { result = resCreateItem.result, message = resCreateItem.message });
        }

        return Json(new JsonViewModel { result = resCreateItem.result, message = resCreateItem.error.error.message });
    }

    [HttpPost]
    public async Task<IActionResult> EditItem([FromBody] EditItemViewModel editItemObj)
    {
        try
        {
            UpdateItemCommand updateItemCommand = MappingUpdateItemCommand(editItemObj);
            BaseResponse<CommandResponse> resUpdateItem = await _itemAPI.UpdateItemAsync(updateItemCommand);
            if (resUpdateItem.result)
            {
                return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.message });
            }
            return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditItemInBranch([FromBody] EditItemViewModel editItemObj)
    {
        try
        {
            UpdateItemInBranchCommand updateItemCommand = MappingUpdateItemInBranchCommand(editItemObj);
            BaseResponse<CommandResponse> resUpdateItem = await _itemInBranchAPI.UpdateItemInBranchAsync(updateItemCommand);
            if (resUpdateItem.result)
            {
                return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.message });
            }
            return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteItem([FromBody] DeleteItemViewModel delItemObj)
    {
        try
        {
            DeleteItemCommand delItemCommand = new DeleteItemCommand { itemid = delItemObj.itemid, deletedby = base.UserProfile.rolename };
            BaseResponse<CommandResponse> resDelItem = await _itemAPI.DeleteItemAsync(delItemCommand);
            if (resDelItem.result)
            {
                return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.message });
            }
            return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteItemInBranch([FromBody] DeleteItemViewModel delItemObj)
    {
        try
        {
            DeleteItemInBranchCommand delItemCommand = new DeleteItemInBranchCommand
            {
                branchid = delItemObj.searchbranchid,
                itemid = delItemObj.itemid,
                updatedby = base.UserProfile.rolename,
                updateddate = DateTime.Now
            };
            BaseResponse<CommandResponse> resDelItem = await _itemInBranchAPI.DeleteItemInBranchAsync(delItemCommand);
            if (resDelItem.result)
            {
                return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.message });
            }
            return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<JsonResult> SearchItemByBranch(int branchid)
    {
        try
        {
            BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItem = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(branchid);
            if (!resItem.result)
            {
                throw new Exception(resItem.error.error.message);
            }
            //Mapping Data
            List<GetItemListResponseDTO> resData = _mapper.Map<List<GetItemListResponseDTO>>(resItem.data.itemlist);
            resData.ForEach(s =>
            {
                if(branchid == 1)
                {
                    s.isiteminbranch = false;
                    s.searchbranchid = branchid;
                }
                else
                {
                    s.isiteminbranch = true;
                    s.searchbranchid = branchid;
                }
                
            });
            return Json(new { result = true, data = resData, message = "สำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ไม่สามารถดึงข้อมูลสินค้าได้. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> TransferItem([FromBody] ReceiveTransferItemViewModel model)
    {
        #region Validate QTY
        var resValidateQTY = ValidateQTYItemTransfer(model);
        if (!resValidateQTY.result)
        {
            return Json(new { result = false, message = resValidateQTY.message });
        }
        #endregion

        UpdateItemTransferCommand updateItemCommand = PrepareReceiveItemTransferCommand(model);
        BaseResponse<CommandResponse> resUpdateItem = await _itemTransferAPI.ReceiveItemTransferAsync(updateItemCommand);
        if (resUpdateItem.result)
        {
            return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.message });
        }
        return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.error.error.message });
    }


    /// <summary>
    /// ดึงข้อมูล Stock โดยสาขา
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        List<GetItemListResponseDTO> resItemList = null;
        try
        {
            if(base.UserProfile.roleid == (int)EnumModel.UserRole.Admin)
            {
                //สินค้าคลังใหญ่
                BaseResponse<List<GetItemListResponseDTO>> resItem = await _itemAPI.GetItemListAsync();
                if (!resItem.result)
                {
                    throw new Exception(resItem.error.error.message);
                }
                resItemList = resItem.data;
                
            }
            else
            {
                //สินค้าคลังสาขา
                BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);
                if (!resItemInBranch.result)
                {
                    throw new Exception(resItemInBranch.error.error.message);
                }
                //Mapping Data
                resItemList = _mapper.Map<List<GetItemListResponseDTO>>(resItemInBranch.data.itemlist);
            }
            return Json(new { data = resItemList });
        }
        catch
        {
            resItemList = new List<GetItemListResponseDTO>();
            return Json(new { data = resItemList });
        }
    }

    [Route("item/search")]
    [HttpPost]
    public async Task<IActionResult> SearchOrderReport(string searchText)
    {
        //return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data.OrderBy(o => o.SEQ).ToList() });
        var form = Request.Form.ToList();

        BaseResponse<List<GetItemListResponseDTO>> resItemList = await _itemAPI.GetItemListAsync();
        return Json(new { data = resItemList.data });
    }

    [HttpPost]
    public async Task<JsonResult> FillSourceDestinationBranch(int transferTypeID)
    {
        List<SelectListItem> sourceList = null;
        List<SelectListItem> destinationList = null;
        List<SelectListItem> transferItemList = null;
        try
        {
            sourceList = await GetTransferSourcehBranchItemListAsync(transferTypeID);
            destinationList = await GetTransferDestinationBranchItemListAsync(transferTypeID);
            transferItemList = await GetItemTransferItemListByTransferType(transferTypeID);
            //Set Search Data
            //DestinationSearchData = destinationList;
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = "ไม่สามารถดึงข้อมูลสาขาได้, กรุณาลองใหม่อีกครั้ง" });
        }
        return Json(new { result = true, data_source = sourceList, data_destination = destinationList, data_itemlist = transferItemList, msg = "สำเร็จ" });
    }

    [HttpPost]
    public async Task<JsonResult> FillItemTransferByBranchID(int transferTypeID, int branchID)
    {
        List<SelectListItem> destinationList = null;
        List<SelectListItem> transferItemList = null;
        try
        {
            transferItemList = await GetItemTransferItemListByTransferType(transferTypeID, branchID);
            destinationList = await GetTransferDestinationBranchItemListAsync(transferTypeID, branchID);
            //Set Search Data
            //DestinationSearchData = destinationList;
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ไม่สามารถดึงข้อมูลสาขาได้, กรุณาลองใหม่อีกครั้ง | ข้อผิดพลาด -> {ex.Message}" });
        }
        return Json(new { result = true, data_itemlist = transferItemList, data_destination = destinationList, msg = "สำเร็จ" });
    }

    [HttpPost]
    public async Task<ActionResult> UploadFilesAsync()
    {
        string filePath = string.Empty;
        int errCount = 0;
        try
        {
            if (Request.Form.Files.Count > 0)
            {
                List<int> errRow = new List<int>();
                List<ImportItemViewModel> itemList = new List<ImportItemViewModel>();

                IFormFile file = Request.Form.Files[0];

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                using ExcelPackage excelPackage = new ExcelPackage(ms);
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1]; // Assuming the first worksheet

                #region Prepare Data
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        string itemcode = worksheet.Cells[row, 1].GetValue<string>();
                        string itemname = worksheet.Cells[row, 2].GetValue<string>();
                        string itemtype = worksheet.Cells[row, 3].GetValue<string>();
                        string itembrand = worksheet.Cells[row, 4].GetValue<string>();
                        int qty = worksheet.Cells[row, 5].GetValue<int>();
                        decimal cost = worksheet.Cells[row, 6].GetValue<decimal>();
                        decimal price = worksheet.Cells[row, 7].GetValue<decimal>();
                        int minqty = worksheet.Cells[row, 8].GetValue<int>();
                        string description = worksheet.Cells[row, 9].GetValue<string>();
                        if (!string.IsNullOrEmpty(itemcode)
                            && !string.IsNullOrEmpty(itemname)
                            && !string.IsNullOrEmpty(itemtype)
                            && !string.IsNullOrEmpty(itembrand))
                        {
                            itemList.Add(new ImportItemViewModel
                            {
                                itemcode = itemcode,
                                itemname = itemname,
                                itemtype = itemtype,
                                itembrand = itembrand,
                                qty = qty,
                                cost = cost,
                                price = price,
                                minqty = minqty,
                                description = description
                            });
                        }
                        else
                        {
                            //Invalid data
                        }

                    }
                    catch (Exception)
                    {
                        errCount++;
                        errRow.Add(row);
                    }
                }

                if (errRow.Count > 0)
                {
                    return Json(new { result = false, message = $"ไม่สามารถนำเข้าไฟล์สินค้า, ข้อมูลไม่ถุกต้องจำนวน {errCount} แถว, กรุณาตรวจสอบข้อมูลแถวที่ -> {errRow.Aggregate((s, t) => s + ',' + t)}" });
                }
                #endregion

                #region Create item List
                CreateItemListCommand CreateItemListCommand = await MappingCreateItemListCommand(itemList);
                BaseResponse<CommandResponse> resImportItems = await _itemAPI.CreateItemListAsync(CreateItemListCommand);
                if (!resImportItems.result)
                {
                    throw new Exception(resImportItems.error.error.message);
                }
                #endregion

                #region SETUP FILENAME, Save to directory
                await SaveExcelToDirectory(file.FileName, excelPackage);
                #endregion

                return Json(new { result = true, message = "นำเข้าสินค้าสำเร็จ" });
            }

            return Json(new { result = false, message = "ไม่พบไฟล์สินค้า" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ไม่สามารถนำเข้าไฟล์สินค้า, กรุณาลองใหม่อีกครั้ง | ข้อผิดพลาด -> {ex.Message}" });
        }
    }


    private async Task SaveExcelToDirectory(string fileName, ExcelPackage excelPackage)
    {
        try
        {
            await Task.Run(() =>
            {
                //SET FOLDER TO COPY FILE, CREATE IF FOLDER DOES NOT EXISTS
                string filePath = Path.Combine(AppContext.BaseDirectory, _appConfig.GetImportItemFilePath());
                if (!System.IO.Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                //SETUP FILENAME
                string dateRef = DateTime.Now.ToString("yyyyMMddhhmmss");
                filePath = Path.Combine(filePath, dateRef + "_" + fileName);

                //Write content to excel file 
                FileInfo fInfo = new FileInfo(filePath);
                excelPackage.SaveAs(fInfo);
                excelPackage.Dispose();
            });
        }
        catch (Exception ex)
        {
            _log.Error($"ไม่สามารถเขียนไฟล์นำเข้าสินค้า[{fileName}] -> error: {ex.Message}");
        }
    }


    [Obsolete("ไม่ใช้")]
    [HttpPost]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
        throw new NotImplementedException();
        if (file == null || file.Length == 0)
            return BadRequest("Please select a valid Excel file.");

        try
        {
            List<int> errRow = new List<int>();
            List<ImportItemViewModel> itemList = new List<ImportItemViewModel>();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[1]; // Assuming the first worksheet

            int rowCount = worksheet.Dimension.Rows;
            int colCount = worksheet.Dimension.Columns;
            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    var qty = worksheet.Cells[row, 1].GetValue<int>();
                    var itemtype = worksheet.Cells[row, 2].GetValue<string>();
                    var itemcode = worksheet.Cells[row, 3].GetValue<string>();
                    var itembrand = worksheet.Cells[row, 4].GetValue<string>();
                    var itemname = worksheet.Cells[row, 5].GetValue<string>();
                    var description = worksheet.Cells[row, 6].GetValue<string>();
                    itemList.Add(new ImportItemViewModel
                    {
                        qty = qty,
                        itemtype = itemtype,
                        itemcode = itemcode,
                        itembrand = itembrand,
                        itemname = itemname,
                        description = description
                    });
                }
                catch (Exception ex)
                {
                    errRow.Add(row);
                }

                //for (int col = 1; col <= colCount; col++)
                //{
                //	//Parse here
                //	var dsd = worksheet.Cells[row, col].Value;
                //}
            }

            if (itemList.Count != (rowCount - 2))
            {
                return Json(new { result = false, message = $"ไม่สามารถนำเข้าไฟล์สินค้า, กรุณาตรวจสอบข้อมูลแถวที่ -> {errRow.Aggregate((s, t) => s + ',' + t)}" });
            }


            //var cells = worksheet.Cells;
            //var dictionary = cells
            //	.GroupBy(c => new { c.Start.Row, c.Start.Column })
            //	.ToDictionary(
            //		rcg => new KeyValuePair<int, int>(rcg.Key.Row, rcg.Key.Column),
            //		rcg => cells[rcg.Key.Row, rcg.Key.Column].Value);

            // Now, you can access and process the Excel data from the worksheet.
            // Example: Read data from Excel cells
            //var cellValue = worksheet.Cells["A1"].Text; // Replace with the desired cell address

            //return Ok($"Uploaded file successfully. Cell A1 value: {cellValue}");

            return Json(new { result = true, message = $"นำเข้าสินค้าจำนวน {itemList.Count} สำเร็จ" });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ไม่สามารถนำเข้าไฟล์สินค้า, กรุณาลองใหม่อีกครั้ง | ข้อผิดพลาด -> {ex.Message}" });
        }
    }

    public async Task<List<SelectListItem>> GetTransferSourcehBranchItemListAsync(int transferTypeID)
    {
        List<SelectListItem> res = new List<SelectListItem>();
        try
        {
            BaseResponse<List<GetBranchResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();
            if (!resBranch.result)
            {
                return res;
            }

            //คลัง ไป สาขา
            if (transferTypeID == (int)TransferType.WTB)
            {
                res = (from a in resBranch.data
                       where a.branchid == 1
                       select new SelectListItem
                       {
                           Text = a.branchname,
                           Value = a.branchid.ToString()
                       }).ToList();

                //res.Add(new SelectListItem
                //{
                //    Text = "คลังสินค้าสำนักงานใหญ่",
                //    Value = "99",
                //});
            }
            else
            {
                res = (from a in resBranch.data
                       select new SelectListItem
                       {
                           Text = a.branchname,
                           Value = a.branchid.ToString()
                       }).ToList();
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex.Message, ex);
        }
        return res;
    }

    public async Task<List<SelectListItem>> GetTransferDestinationBranchItemListAsync(int transferTypeID, int filterOutBranchID = 0)
    {
        List<SelectListItem> res = new List<SelectListItem>();
        try
        {
            BaseResponse<List<GetBranchResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();
            if (!resBranch.result)
            {
                return res;
            }
            //คลัง ไป สาขา
            if (transferTypeID == (int)TransferType.WTW)
            {
                res = (from a in resBranch.data
                       where a.branchid == 1
                       select new SelectListItem
                       {
                           Text = a.branchname,
                           Value = a.branchid.ToString()
                       }).ToList();
            }
            else
            {
                //Filter out branch
                if (filterOutBranchID > 0)
                {
                    resBranch.data = resBranch.data.Where(w => w.branchid != filterOutBranchID).ToList();
                }
                res = (from a in resBranch.data
                       select new SelectListItem
                       {
                           Text = a.branchname,
                           Value = a.branchid.ToString()
                       }).ToList();
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex.Message, ex);
        }
        return res;
    }

    public async Task<List<SelectListItem>> GetItemTransferItemListByTransferType(int transferTypeID, int brnchID = 0)
    {
        List<int> branchList = new List<int>();
        if (transferTypeID == (int)TransferType.WTB)
        {
            var resItem = await _itemAPI.GetItemListAsync();
            if (!resItem.result)
            {
                return new List<SelectListItem>();
            }
            return (from a in resItem.data
                    select new SelectListItem
                    {
                        Text = a.name,
                        Value = a.itemid.ToString()
                    }).ToList();
        }
        else
        {
            if (brnchID == 0)
            {
                return new List<SelectListItem>();
            }
            var resItemBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(brnchID);
            if (!resItemBranch.result)
            {
                return new List<SelectListItem>();
            }
            return (from a in resItemBranch.data.itemlist
                    select new SelectListItem
                    {
                        Text = a.itemname,
                        Value = a.itemid.ToString()
                    }).ToList();
        }
    }

    public async Task<List<SelectListItem>> PrepareSelectBranch()
    {
        var resBranch = await _branchAPI.GetBranchListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }

    public async Task<List<SelectListItem>> PrepareSelectItemTransferType()
    {
        BaseResponse<List<GetTransferTypeListResponseDTO>> resItemTransaferTypeList = await _itemTransferAPI.GetItemTransferTypeAsync();
        return resItemTransaferTypeList.data.Select(s => new SelectListItem { Text = s.transfertypename, Value = s.transfertypeid.ToString() }).ToList();

    }

    #region Private Method
    private CreateItemCommand MappingCreateItemCommand(AddItemViewModel itemViewModel)
    {
        return new CreateItemCommand
        {
            itemcode = itemViewModel.ItemCode,
            itemtypeid = itemViewModel.ItemTypeID,
            brandid = itemViewModel.BrandID,
            unitofmeasureid = itemViewModel.UnitOfMeasureID,
            name = itemViewModel.Name,
            barcode = itemViewModel.BarCode,
            description = itemViewModel.Description,
            //shortname = !string.IsNullOrEmpty(itemViewModel.ShortName) ? itemViewModel.ShortName : itemViewModel.Name,
            itemimageurl = !string.IsNullOrEmpty(itemViewModel.ItemImageUrl) ? itemViewModel.ItemImageUrl : "../assets/img/product/noimage.png",
            price = itemViewModel.Price,
            qty = itemViewModel.Qty,
            notifyminqty = itemViewModel.NotifyMinQty,
            createdby = base.UserProfile.rolename,
            isactive = bool.TryParse(itemViewModel.IsActive, out bool isactive) && isactive,
        };
    }

    private async Task<CreateItemListCommand> MappingCreateItemListCommand(List<ImportItemViewModel> itemViewModel)
    {
        List<int> errRow = new List<int>();
        int rowCount = 2;
        #region Mapping ItemTypeID by itemtype, Mapping ItemBrandID by itembrand
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemType = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrand = await _itemBrandAPI.GetItemBrandListAsync();
        BaseResponse<List<GetItemListResponseDTO>> resItems = await _itemAPI.GetItemListAsync();

        List<CreateItemDetailCommand> createItemDetailCommands = new List<CreateItemDetailCommand>();
        itemViewModel.ForEach(s =>
        {
            //Check isexist itemcode
            GetItemListResponseDTO item = resItems.data?.FirstOrDefault(w => w.itemcode.Trim().ToLower() == s.itemcode.Trim().ToLower());
            if (item != null)
            {
                s.isupdate = true;
            }

            //Check isexist itemtype
            GetItemTypeListResponseDTO itemType = resItemType.data?.FirstOrDefault(w => w.itemtypename.Trim().ToLower() == s.itemtype.Trim().ToLower());
            if (itemType == null)
            {
                errRow.Add(rowCount);
                throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า, กรุณาตรวจสอบข้อมูลประเภทสินค้าแถวที่ -> {rowCount}");
            }

            //Check isexist itembrand
            GetItemBrandListResponseDTO itemBrand = resItemBrand.data?.FirstOrDefault(w => w.brandname.Trim().ToLower() == s.itembrand.Trim().ToLower());
            if (itemBrand == null)
            {
                errRow.Add(rowCount);
                throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า, กรุณาตรวจสอบข้อมูลแบรนด์สินค้าแถวที่ -> {rowCount}");
            }

            CreateItemDetailCommand itemEnt = new CreateItemDetailCommand
            {
                itemcode = s.itemcode,
                itemtypeid = itemType.itemtypeid,
                brandid = itemBrand.brandid,
                unitofmeasureid = 1,
                name = s.itemname,
                //barcode = null,
                description = s.description,
                //shortname = !string.IsNullOrEmpty(s.itemname) ? s.itemname : s.itemname,
                itemimageurl = "../assets/img/product/noimage.png",
                price = s.price,
                qty = s.qty,
                notifyminqty = s.minqty,
                createdby = base.UserProfile.rolename,
                isactive = true,
                discountpercent = 0,
                isupdate = s.isupdate,
                cost = s.cost
            };
            createItemDetailCommands.Add(itemEnt);
            rowCount++;
        });
        #endregion

        return new CreateItemListCommand
        {
            items = createItemDetailCommands
        };
    }

    private UpdateItemCommand MappingUpdateItemCommand(EditItemViewModel itemViewModel)
    {
        return new UpdateItemCommand
        {
            itemid = itemViewModel.ItemID,
            name = itemViewModel.Name,
            barcode = itemViewModel.BarCode,
            description = itemViewModel.Description,
            //shortname = !string.IsNullOrEmpty(itemViewModel.ShortName) ? itemViewModel.ShortName : itemViewModel.Name,
            itemimageurl = !string.IsNullOrEmpty(itemViewModel.ItemImageUrl) ? itemViewModel.ItemImageUrl : "../assets/img/product/noimage.png",
            qty = itemViewModel.Qty,
            notifyqty = itemViewModel.NotifyMinQty,
            discountpercent = itemViewModel.DiscountPercent,
            price = itemViewModel.Price,
            updatedby = base.UserProfile.rolename,
            isactive = bool.TryParse(itemViewModel.IsActive, out bool isactive) && isactive
        };
    }

    private UpdateItemInBranchCommand MappingUpdateItemInBranchCommand(EditItemViewModel itemViewModel)
    {
        return new UpdateItemInBranchCommand
        {
            branchid = itemViewModel.BranchID,
            itemid = itemViewModel.ItemID,
            price = itemViewModel.Price,
            qty = itemViewModel.Qty,
            updatedby = base.UserProfile.rolename,
            updateddate = DateTime.Now
        };
    }

    private EditItemViewModel EditItemMapping(GetItemListResponseDTO itemResponseDTO)
    {
        EditItemViewModel editItemViewModel = _mapper.Map<EditItemViewModel>(itemResponseDTO);
        return editItemViewModel;
    }

    private EditItemViewModel EditItemInBranchMapping(GetItemInBranchByBranchIDItemResponseDTO itemResponseDTO)
    {
        EditItemViewModel itemViewModel = _mapper.Map<EditItemViewModel>(itemResponseDTO);
        return itemViewModel;
    }
    private ReceiveTransferItemViewModel ReceiveTransferMapping(GetItemTransferResponseDTO itemTransferDTO)
    {
        ReceiveTransferItemViewModel receiveTransferItemView = _mapper.Map<ReceiveTransferItemViewModel>(itemTransferDTO);
        return receiveTransferItemView;
    }

    private CreateItemTransferCommand CreateItemTransferCommand(TransferItemViewModel reqObj, List<CreateItemTransferDetailCommand> itemsTransfer)
    {
        return new CreateItemTransferCommand
        {
            transfertypeid = reqObj.transfertypeid,
            sourceid = reqObj.source_branchid.ToInt32(),
            destinationid = reqObj.destination_branchid.ToInt32(),
            description = reqObj.description,
            createdby = base.UserProfile.username,
            creadeddate = DateTime.Now,
            transferstatus = (int)TransferStatus.Pending,
            isactive = true,
            items = itemsTransfer
        };
    }

    private BaseResponse<bool> ValidateQTYItemTransfer(ReceiveTransferItemViewModel viewModel)
    {
        if (viewModel.QTY == 0)
        {
            return new BaseResponse<bool> { message = "ไม่สามารถทำรายการได้ เนื่องจากจำนวนโอนสินค้าไม่ถูกต้อง" };
        }

        if (viewModel.QTY != (viewModel.ReceiveQTY + viewModel.ReturnQTY))
        {
            return new BaseResponse<bool> { message = "ไม่สามารถทำรายการได้ เนื่องจากจำนวนรับโอนสินค้าไม่ถูกต้อง" };
        }
        return new BaseResponse<bool> { result = true, message = "Success" };
    }

    private UpdateItemTransferCommand PrepareReceiveItemTransferCommand(ReceiveTransferItemViewModel viewModel)
    {
        UpdateItemTransferCommand updateItemTransferCommand = new UpdateItemTransferCommand
        {
            transferid = viewModel.TransferID,
            sourceid = viewModel.SourceID,
            destinationid = viewModel.DestinationID,
            itemid = viewModel.ItemID,
            qty = viewModel.QTY,
            receiveqty = viewModel.ReceiveQTY,
            returnqty = viewModel.ReturnQTY,
            description = viewModel.Description,
            updatedby = base.UserProfile.username,
            updateddate = DateTime.Now,
            transferstatusid = viewModel.TransferStatusID
        };
        return updateItemTransferCommand;
    }
    #endregion


}
