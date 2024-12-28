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
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using OfficeOpenXml;
using CYRetailIMS.Application.Common.Confiuration;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;
using NetBarcode;
using Type = NetBarcode.Type;
using CYRetailIMS.Infrastructure.Common.Extensions;
using NUglify.Helpers;
using Microsoft.AspNetCore.Authorization;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByBarcode.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchForImportByBranchID.v1;
using CYRetailIMS.Application.ExternalService.SubItemTypeAPI;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
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
    private readonly ISubItemTypeAPI _subItemTypeAPI;
    private string _sessionTempTransferItemName => "TEMP_TRANSFER_ITEM_DATA";

    public ItemController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IAppConfig appConfig,
        IWebHostEnvironment webHostEnvironment,
        IItemAPI itemAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTypeAPI itemTypeAPI,
        IItemUnitOfMeasureAPI itemUnitOfMeasureAPI,
        IBranchAPI branchAPI,
        IItemTransferAPI itemTransferAPI,
        IItemInBranchAPI itemInBranchAPI,
        ISubItemTypeAPI subItemTypeAPI) : base(httpClientRequest, mapper, log)
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
        _subItemTypeAPI = subItemTypeAPI;
    }

    public async Task<IActionResult> Index()
    {
        BaseResponse<List<GetBranchResponseDTO>> resBranchList = null;
        if (base.UserProfile.roleid == (int)EnumModel.UserRole.Admin || base.UserProfile.roleid == (int)EnumModel.UserRole.Stock)
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

    public async Task<IActionResult> ImportAsync()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public IActionResult Adjust()
    {
        return View();
    }

    public async Task<IActionResult> Transfer()
    {
        //BaseResponse<List<GetItemListResponseDTO>> resItemList = await _itemAPI.GetItemListAsync();
        //ViewBag.ItemList = resItemList;
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ItemTransferList = await GetItemTransferItemListByTransferType((int)TransferType.WTB);
        ViewBag.ItemTransaferTypeList = await PrepareSelectItemTransferType(); ;
        return View();
    }

    public async Task<IActionResult> TransferHistory()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ItemTransferStatus = await PrepareSelectItemTransferStatus();
        return View();
    }

    public async Task<IActionResult> InventoryTransferHistory()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ItemBrandList = await PrepareSelectBrand();
        return View();
    }

    /// <summary>
    /// Default Item Transfer History
    /// Allow only Admin, Stock
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetItemTransferHistory()
    {
        BaseResponse<List<GetItemTransferResponseDTO>> transferHistory = null;
        try
        {
            if (base.UserProfile.roleid == (int)EnumModel.UserRole.Admin || base.UserProfile.roleid == (int)EnumModel.UserRole.Stock)
            {
                transferHistory = await _itemTransferAPI.GetItemTransferForAdminAsync(new GetItemTransferListQuery());
            }
            else
            {
                transferHistory = await _itemTransferAPI.GetItemTransferByDestinationBranchIDAsync(new GetItemTransferByDestinationBranchIDQuery
                {
                    destinationbranchid = base.UserProfile.access_branch.FirstOrDefault().branchid
                });
            }
            if (!transferHistory.result)
            {
                return Json(new { data = new List<GetItemTransferResponseDTO>() });
            }
            return Json(new { data = transferHistory.data });
        }
        catch
        {
            return Json(new { data = new List<GetItemTransferResponseDTO>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SearchItemTransferHistory([FromBody] SearchItemTransferHistoryViewModel searchItem)
    {
        BaseResponse<List<GetItemTransferResponseDTO>> transferHistory = null;
        try
        {
            #region Prepare Search Start & End Date
            DateTime? transferSrtartDate = null;
            DateTime? transferEndDate = null;
            if (!string.IsNullOrEmpty(searchItem.transferstartdate))
            {
                string[] sTransferDate = searchItem.transferstartdate.Split("-");
                if (sTransferDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                transferSrtartDate = new DateTime(sTransferDate[2].ToInt32(), sTransferDate[1].ToInt32(), sTransferDate[0].ToInt32());
            }

            if (!string.IsNullOrEmpty(searchItem.transferenddate))
            {
                string[] sTransferEndDate = searchItem.transferenddate.Split("-");
                if (sTransferEndDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                transferEndDate = new DateTime(sTransferEndDate[2].ToInt32(), sTransferEndDate[1].ToInt32(), sTransferEndDate[0].ToInt32());
            }
            if((transferSrtartDate.HasValue && transferEndDate.HasValue) 
                && DateTime.Compare(transferSrtartDate.Value, transferEndDate.Value) == 1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            #endregion

            if (base.UserProfile.roleid == (int)UserRole.Admin || base.UserProfile.roleid == (int)UserRole.Stock)
            {
                transferHistory = await _itemTransferAPI.GetItemTransferForAdminAsync(new GetItemTransferListQuery
                {
                    branchid = searchItem.branchid,
                    transferstartdate = transferSrtartDate,
                    transferenddate = transferEndDate,
                    transferstatusid = searchItem.transferstatusid
                });
            }
            else
            {
                transferHistory = await _itemTransferAPI.GetItemTransferByDestinationBranchIDAsync(new GetItemTransferByDestinationBranchIDQuery
                {
                    destinationbranchid = base.UserProfile.access_branch.FirstOrDefault().branchid,
                    transferstartdate = transferSrtartDate,
                    transferenddate = transferEndDate,
                    transferstatusid = searchItem.transferstatusid
                });
            }
            if (!transferHistory.result)
            {
                return Json(new { result = false, message = $"ไม่พบข้อมูลการโอนสินค้า", data = new List<GetItemTransferResponseDTO>() });
            }

            if (transferHistory.data.Count == 0)
            {
                return Json(new { result = false, message = $"ไม่พบข้อมูลการโอนสินค้า", data = new List<GetItemTransferResponseDTO>() });
            }
            return Json(new { result = true, data = transferHistory.data, message = "สำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<GetItemTransferResponseDTO>() });
        }
    }


    public async Task<IActionResult> BarcodeTransferAsync()
    {
        #region Get- Set Item
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await GetItemSessionDataAsync();
        #endregion
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ItemTransferList = await GetItemTransferItemListByTransferType((int)TransferType.WTB);
        ViewBag.ItemTransaferTypeList = await PrepareSelectItemTransferType(); ;
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
        viewModel.BarCodeBase64 = GenerateItemBarcode(viewModel.BarCode);

        //Get Master Data
        //BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();
        //BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();
        //BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _itemUnitOfMeasureAPI.GetUnitOfMeasureListAsync();

        ViewBag.ItemTypeList = await PrepareSelectItemType();
        ViewBag.ItemBrandList = await PrepareSelectBrand();
        ViewBag.ItemUOMList = await PrepareSelectUnitOfMeasure();
        ViewBag.SubItemTypeList = await PrepareSelectSubItemType();
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
        //BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();
        //BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();
        //BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _itemUnitOfMeasureAPI.GetUnitOfMeasureListAsync();

        ViewBag.ItemTypeList = await PrepareSelectItemType();
        ViewBag.ItemBrandList = await PrepareSelectBrand();
        ViewBag.ItemUOMList = await PrepareSelectUnitOfMeasure();
        ViewBag.SubItemTypeList = await PrepareSelectSubItemType();

        return View(viewModel);
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
            return Json(new { result = false, msg = $"ขออภัย มีบางอย่างผิดพลาด กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveBarcodeTransaferItemAsync(TransferItemViewModel transferItemObj)
    {
        try
        {
            List<TransferItemDetailViewModel> tempList = HttpContext.Session.GetDataFromSession<List<TransferItemDetailViewModel>>(_sessionTempTransferItemName);
            if (tempList == null || tempList?.Count == 0)
            {
                return Json(new { result = false, msg = "ข้อมูลไม่ถูกต้อง, กรุณาตรวจสอบข้อมูลใหม่อีกครั้ง!" });
            }
            List<CreateItemTransferDetailCommand> itemTransferList = tempList.Select(s => new CreateItemTransferDetailCommand
            {
                itemid = s.nitemid,
                qty = s.nqty
            }).ToList();
            CreateItemTransferCommand createItemTransferCommand = CreateItemTransferCommand(transferItemObj, itemTransferList);
            BaseResponse<CommandResponse> resCreateTrn = await _itemTransferAPI.CreateItemTransferAsync(createItemTransferCommand);
            if (!resCreateTrn.result)
            {
                return Json(new { result = false, msg = resCreateTrn.error.error.message });
            }

            //Clear TEMP_TRANSFER_ITEM_DATA
            HttpContext.Session.Remove(_sessionTempTransferItemName);
            return Json(new { result = true, msg = "บันทึกข้อมูลสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย มีบางอย่างผิดพลาด, กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveDraftTransaferItemAsync([FromBody] DraftTransferItemViewModel transferItemObj)
    {
        try
        {

            List<TransferItemDetailViewModel> tempList = HttpContext.Session.GetDataFromSession<List<TransferItemDetailViewModel>>(_sessionTempTransferItemName);
            if (tempList == null || tempList?.Count == 0)
            {
                return Json(new { result = false, msg = "ข้อมูลไม่ถูกต้อง, กรุณาตรวจสอบข้อมูลใหม่อีกครั้ง!" });
            }
            List<CreateItemTransferDetailCommand> itemTransferList = tempList.Select(s => new CreateItemTransferDetailCommand
            {
                itemid = s.nitemid,
                qty = s.nqty
            }).ToList();
            CreateItemTransferCommand createItemTransferCommand = CreateItemTransferCommand(transferItemObj, itemTransferList);
            BaseResponse<CommandResponse> resCreateTrn = await _itemTransferAPI.CreateItemTransferAsync(createItemTransferCommand);
            if (!resCreateTrn.result)
            {
                return Json(new { result = false, msg = resCreateTrn.error.error.message });
            }

            //Clear TEMP_TRANSFER_ITEM_DATA
            HttpContext.Session.Remove(_sessionTempTransferItemName);
            return Json(new { result = true, msg = "บันทึกข้อมูลสำเร็จ." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = $"ขออภัย มีบางอย่างผิดพลาด, กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
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
        try
        {
            CreateItemCommand createItemCommand = MappingCreateItemCommand(addItemObj);
            BaseResponse<CommandResponse> resCreateItem = await _itemAPI.CreateItemAsync(createItemCommand);
            if (resCreateItem.result)
            {
                return Json(new JsonViewModel { result = resCreateItem.result, message = resCreateItem.message });
            }

            return Json(new JsonViewModel { result = resCreateItem.result, message = resCreateItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
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
            DeleteItemCommand delItemCommand = new DeleteItemCommand { itemid = delItemObj.itemid, deletedby = base.UserProfile.username };
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
                updatedby = base.UserProfile.username,
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
        List<GetItemListResponseDTO> resItemList = null;
        try
        {
            if (branchid == 1)
            {
                //สินค้าคลังใหญ่
                BaseResponse<List<GetItemListResponseDTO>> resItem = await _itemAPI.GetItemListAsync();
                if (!resItem.result)
                {
                    return Json(new { result = false, message = "ไม่มีสินค้าหน้าร้าน", data = new List<GetItemListResponseDTO>() });
                }
                resItemList = resItem.data;

            }
            else
            {
                //สินค้าคลังสาขา
                BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(branchid);
                if (!resItemInBranch.result)
                {
                    return Json(new { result = false, message = "ไม่มีสินค้าหน้าร้าน", data = new List<GetItemListResponseDTO>() });
                }
                //Mapping Data
                resItemList = _mapper.Map<List<GetItemListResponseDTO>>(resItemInBranch.data.itemlist);
            }

            resItemList.ForEach(s =>
            {
                if (branchid == 1)
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
            return Json(new { result = true, data = resItemList, message = "สำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<GetItemListResponseDTO>() });
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
            if (base.UserProfile.roleid == (int)EnumModel.UserRole.Admin || base.UserProfile.roleid == (int)EnumModel.UserRole.Stock)
            {
                //สินค้าคลังใหญ่
                BaseResponse<List<GetItemListResponseDTO>> resItem = await _itemAPI.GetItemListAsync();
                if (!resItem.result)
                {
                    return Json(new { result = false, message = "ไม่มีสินค้าหน้าร้าน", data = new List<GetItemListResponseDTO>() });
                }
                resItemList = resItem.data;

            }
            else
            {
                //สินค้าคลังสาขา
                BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(base.UserProfile.access_branch.FirstOrDefault().branchid);
                if (!resItemInBranch.result)
                {
                    return Json(new { result = false, message = "ไม่มีสินค้าหน้าร้าน", data = new List<GetItemListResponseDTO>() });
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
            //Branch
            string branchid = Request.Form.FirstOrDefault(w => w.Key == "branchid").Value;
            if(string.IsNullOrEmpty(branchid) || branchid.ToInt32() <= 0)
            {
                return Json(new { result = false, message = "กรุณาระบุสาขาก่อนทำรายการ!" });
            }

            if (Request.Form.Files.Count > 0)
            {
                List<int> errRow = new List<int>();
                List<ImportItemViewModel> itemList = new List<ImportItemViewModel>();

                IFormFile file = Request.Form.Files[0];

                //Branch
                //string branchid = Request.Form.FirstOrDefault(w => w.Key == "branchid").Value;

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
                        string subitemtypecode = worksheet.Cells[row, 4].GetValue<string>();
                        string itembrand = worksheet.Cells[row, 5].GetValue<string>();
                        int qty = worksheet.Cells[row, 6].GetValue<int>();
                        decimal cost = worksheet.Cells[row, 7].GetValue<decimal>();
                        decimal price = worksheet.Cells[row, 8].GetValue<decimal>();
                        int minqty = worksheet.Cells[row, 9].GetValue<int>();
                        int maxqty = worksheet.Cells[row, 10].GetValue<int>();
                        string description = worksheet.Cells[row, 11].GetValue<string>();
                        if (!string.IsNullOrEmpty(itemcode)
                            && !string.IsNullOrEmpty(itemname)
                            && !string.IsNullOrEmpty(itemtype)
                            && !string.IsNullOrEmpty(itembrand))
                        {
                            itemList.Add(new ImportItemViewModel
                            {
                                itemcode = itemcode,
                                subitemtypecode = subitemtypecode,
                                itemname = itemname,
                                itemtype = itemtype,
                                itembrand = itembrand,
                                qty = qty,
                                cost = cost,
                                price = price,
                                minqty = minqty,
                                maxqty = maxqty,
                                description = description
                            });
                        }
                        else
                        {
                            //Invalid data
                        }

                    }
                    catch (Exception ex)
                    {
                        _log.Error($"UploadFilesAsync -> Invalid data row: {row} | err_msg: {ex.Message}");
                        errCount++;
                        errRow.Add(row);
                    }
                }

                if (errRow.Count > 0)
                {
                    return Json(new { result = false, message = $"ไม่สามารถนำเข้าไฟล์สินค้า, ข้อมูลไม่ถุกต้องจำนวน {errCount} แถว, กรุณาตรวจสอบข้อมูลแถวที่ -> {errRow.Select(s => s.ToString()).ToList().Aggregate((s, t) => s + ", " + t)}" });
                }
                #endregion

                #region Create item List
                if (branchid.ToInt32() == 1)
                {
                    CreateItemListCommand CreateItemListCommand = await MappingCreateItemListCommand(itemList);
                    BaseResponse<CommandResponse> resImportItems = await _itemAPI.CreateItemListAsync(CreateItemListCommand);
                    if (!resImportItems.result)
                    {
                        throw new Exception(resImportItems.error.error.message);
                    }
                }
                else
                {
                    CreateItemInBranchListCommand CreateItemListCommand = await MappingCreateItemInBranchListCommand(branchid.ToInt32(), itemList);
                    BaseResponse<CommandResponse> resImportItems = await _itemInBranchAPI.CreateItemInBranchListAsync(CreateItemListCommand);
                    if (!resImportItems.result)
                    {
                        throw new Exception(resImportItems.error.error.message);
                    }
                }
                
                #endregion

                #region SETUP FILENAME, Save to directory
                await SaveExcelToDirectory(file.FileName, excelPackage);
                #endregion

                return Json(new { result = true, message = "นำเข้าสินค้าสำเร็จ" });
            }

            return Json(new { result = false, message = "ไม่พบไฟล์นำเข้าสินค้า" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message });
        }
    }

    #region Private Method
    private async Task<CreateItemListCommand> MappingCreateItemListCommand(List<ImportItemViewModel> itemViewModel)
    {
        List<int> errRow = new List<int>();
        int rowCount = 2;
        #region Mapping ItemTypeID by itemtype, Mapping ItemBrandID by itembrand
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemType = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrand = await _itemBrandAPI.GetItemBrandListAsync();
        BaseResponse<List<GetItemListResponseDTO>> resItems = await _itemAPI.GetItemListAsync();
        BaseResponse<List<GetSubItemTypeResponseDTO>> resSubItemType = await _subItemTypeAPI.GetSubItemTypeListAsync();

        List<CreateItemDetailCommand> createItemDetailCommands = new List<CreateItemDetailCommand>();
        try
        {
            itemViewModel.ForEach(s =>
            {
                //Check isexist itemcode
                GetItemListResponseDTO item = resItems.data?.FirstOrDefault(w => w.itemcode.Trim() == s.itemcode.Trim());
                if (item != null)
                {
                    s.isupdate = true;
                }

                //Check subitemtype
                GetSubItemTypeResponseDTO subItemType = null;
                if (!string.IsNullOrEmpty(s.subitemtypecode))
                {
                    subItemType = resSubItemType.data?.FirstOrDefault(w => w.subitemcode.Trim().ToLower() == s.subitemtypecode.Trim().ToLower());
                    if (subItemType == null)
                    {
                        errRow.Add(rowCount);
                        throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบระเภทย่อยสินค้าแถวที่ -> {rowCount}");
                    }
                }

                //Check isexist itemtype
                GetItemTypeListResponseDTO itemType = resItemType.data?.FirstOrDefault(w => w.itemtypename.Trim().ToLower() == s.itemtype.Trim().ToLower());
                if (itemType == null)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลประเภทสินค้าแถวที่ -> {rowCount}");
                }

                //Check isexist itembrand
                GetItemBrandListResponseDTO itemBrand = resItemBrand.data?.FirstOrDefault(w => w.brandname.Trim().ToLower() == s.itembrand.Trim().ToLower());
                if (itemBrand == null)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลแบรนด์สินค้าแถวที่ -> {rowCount}");
                }

                //Check Description length
                if (s.itemcode.Length > 12)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลรหัสสินค้าแถวที่ -> {rowCount}");
                }

                if (s.itemname.Length > 100)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลชื่อสินค้าแถวที่ -> {rowCount}");
                }

                if (!string.IsNullOrEmpty(s.description) && s.description.Length > 200)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลรายละเอียดสินค้าแถวที่ -> {rowCount}");
                }

                CreateItemDetailCommand itemEnt = new CreateItemDetailCommand
                {
                    itemcode = s.itemcode,
                    itemtypeid = itemType.itemtypeid,
                    subitemtypeid = subItemType != null ? subItemType.subitemtypeid : null,
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
                    notifymaxqty = s.maxqty,
                    createdby = base.UserProfile.username,
                    isactive = true,
                    discountpercent = 0,
                    isupdate = s.isupdate,
                    cost = s.cost
                };
                createItemDetailCommands.Add(itemEnt);
                rowCount++;
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"ขออภัย, เกิดข้อผิดพลาด<br>{ex.Message}");
        }
        #endregion

        return new CreateItemListCommand
        {
            items = createItemDetailCommands
        };
    }

    private async Task<CreateItemInBranchListCommand> MappingCreateItemInBranchListCommand(int branchID, List<ImportItemViewModel> itemViewModel)
    {
        List<int> errRow = new List<int>();
        int rowCount = 2;
        #region Mapping ItemTypeID by itemtype, Mapping ItemBrandID by itembrand
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemType = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrand = await _itemBrandAPI.GetItemBrandListAsync();
        BaseResponse<List<GetItemInBranchForImportByBranchIDResponseDTO>> resItemsInBranch = await _itemInBranchAPI.GetItemBranchForImportAsync(new GetItemInBranchForImportByBranchIDQuery
        {
            branchid = branchID
        });
        BaseResponse<List<GetItemListResponseDTO>> resItems = await _itemAPI.GetItemListAsync();
        //BaseResponse<List<GetSubItemTypeResponseDTO>> resSubItemType = await _subItemTypeAPI.GetSubItemTypeListAsync();


        List<CreateItemInBranchDetailCommand> createItemInBranchDetailCommands = new List<CreateItemInBranchDetailCommand>();
        try
        {
            itemViewModel.ForEach(s =>
            {
                //Check isexist itemcode
                GetItemListResponseDTO item = resItems.data?.FirstOrDefault(w => w.itemcode.Trim() == s.itemcode.Trim());
                var itembranch = resItemsInBranch.data?.FirstOrDefault(w => w.itemid == item.itemid);
                if (itembranch != null)
                {
                    s.isupdate = true;
                }

                //Check subitemtype
                //GetSubItemTypeResponseDTO subItemType = resSubItemType.data?.FirstOrDefault(w => w.subitemcode.Trim().ToLower() == s.subitemtypecode.Trim().ToLower());
                //if (subItemType == null)
                //{
                //    errRow.Add(rowCount);
                //    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า, กรุณาตรวจสอบระเภทย่อยสินค้าแถวที่ -> {rowCount}");
                //}

                //Check isexist itemtype
                GetItemTypeListResponseDTO itemType = resItemType.data?.FirstOrDefault(w => w.itemtypename.Trim().ToLower() == s.itemtype.Trim().ToLower());
                if (itemType == null)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลประเภทสินค้าแถวที่ -> {rowCount}");
                }

                //Check isexist itembrand
                GetItemBrandListResponseDTO itemBrand = resItemBrand.data?.FirstOrDefault(w => w.brandname.Trim().ToLower() == s.itembrand.Trim().ToLower());
                if (itemBrand == null)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลแบรนด์สินค้าแถวที่ -> {rowCount}");
                }

                //Check Description length
                if (s.itemcode.Length > 12)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลรหัสสินค้าแถวที่ -> {rowCount}");
                }

                if (s.itemname.Length > 100)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลชื่อสินค้าแถวที่ -> {rowCount}");
                }

                if (!string.IsNullOrEmpty(s.description) && s.description.Length > 200)
                {
                    errRow.Add(rowCount);
                    throw new Exception($"ไม่สามารถนำเข้าไฟล์สินค้า<br>กรุณาตรวจสอบข้อมูลรายละเอียดสินค้าแถวที่ -> {rowCount}");
                }

                CreateItemInBranchDetailCommand itemEnt = new CreateItemInBranchDetailCommand
                {
                    branchid = branchID,
                    itemid = item.itemid,
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
                    notifymaxqty = s.maxqty,
                    createdby = base.UserProfile.username,
                    isactive = true,
                    discountpercent = 0,
                    isupdate = s.isupdate,
                    cost = s.cost
                };
                createItemInBranchDetailCommands.Add(itemEnt);
                rowCount++;
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"ขออภัย, เกิดข้อผิดพลาด<br>{ex.Message}");
        }
        #endregion

        return new CreateItemInBranchListCommand
        {
            branchid = branchID,
            items = createItemInBranchDetailCommands
        };
    }

    private CreateItemCommand MappingCreateItemCommand(AddItemViewModel itemViewModel)
    {
        if (itemViewModel.NotifyMaxQty < itemViewModel.NotifyMinQty)
        {
            throw new Exception("จำนวนสินค้าขั้นสูงต้องมากกว่าจำนวนขั้นต่ำ");
        }

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
            notifymaxqty = itemViewModel.NotifyMaxQty,
            createdby = base.UserProfile.username,
            isactive = bool.TryParse(itemViewModel.IsActive, out bool isactive) && isactive,
        };
    }

    private UpdateItemCommand MappingUpdateItemCommand(EditItemViewModel itemViewModel)
    {
        if(itemViewModel.NotifyMaxQty < itemViewModel.NotifyMinQty)
        {
            throw new Exception("จำนวนสินค้าขั้นสูงต้องมากกว่าจำนวนขั้นต่ำ");
        }

        return new UpdateItemCommand
        {
            itemid = itemViewModel.ItemID,
            subitemid = itemViewModel.SubItemTypeID.HasValue ? itemViewModel.SubItemTypeID.Value : null,
            name = itemViewModel.Name,
            barcode = itemViewModel.BarCode,
            description = itemViewModel.Description,
            //shortname = !string.IsNullOrEmpty(itemViewModel.ShortName) ? itemViewModel.ShortName : itemViewModel.Name,
            itemimageurl = !string.IsNullOrEmpty(itemViewModel.ItemImageUrl) ? itemViewModel.ItemImageUrl : "../assets/img/product/noimage.png",
            qty = itemViewModel.Qty,
            notifyqty = itemViewModel.NotifyMinQty,
            notifymaxqty = itemViewModel.NotifyMaxQty,
            discountpercent = itemViewModel.DiscountPercent,
            price = itemViewModel.Price,
            updatedby = base.UserProfile.username,
            isactive = bool.TryParse(itemViewModel.IsActive, out bool isactive) && isactive
        };
    }

    private UpdateItemInBranchCommand MappingUpdateItemInBranchCommand(EditItemViewModel itemViewModel)
    {
        if(itemViewModel.NotifyMinQty < 0)
        {
            throw new Exception("จำนวนขั้นต่ำไม่น้อยกว่า 0");
        }

        if (itemViewModel.NotifyMaxQty < itemViewModel.NotifyMinQty)
        {
            throw new Exception("จำนวนสินค้าขั้นสูงต้องมากกว่าจำนวนขั้นต่ำ");
        }

        return new UpdateItemInBranchCommand
        {
            branchid = itemViewModel.BranchID,
            itemid = itemViewModel.ItemID,
            price = itemViewModel.Price,
            qty = itemViewModel.Qty,
            notifyminqty = itemViewModel.NotifyMinQty,
            notifymaxqty = itemViewModel.NotifyMaxQty,
            updatedby = base.UserProfile.username,
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
            createddate = DateTime.Now,
            transferstatus = (int)TransferStatus.Pending,
            isactive = true,
            items = itemsTransfer
        };
    }

    private CreateItemTransferCommand CreateItemTransferCommand(DraftTransferItemViewModel reqObj, List<CreateItemTransferDetailCommand> itemsTransfer)
    {
        return new CreateItemTransferCommand
        {
            transfertypeid = reqObj.transfertypeid,
            sourceid = reqObj.source_branchid.ToInt32(),
            destinationid = reqObj.destination_branchid.ToInt32(),
            description = reqObj.description,
            createdby = base.UserProfile.username,
            createddate = DateTime.Now,
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
            transferstatusid = viewModel.TransferStatusID,
            transferheaderid = viewModel.TransferHeaderID
        };
        return updateItemTransferCommand;
    }
    #endregion

    #region Generate Barcode
    private string? GenerateItemBarcode(string sBarcode)
    {
        try
        {
            //Barcode b = new Barcode(sBarcode, BarcodeStandard.Type.Code93);
            //b.IncludeLabel = true;
            //Image img = b.Encode(BarcodeStandard.Type.Code93, "038000356216");
            if (string.IsNullOrEmpty(sBarcode))
            {
                return default;
            }
            //var barcode = new Barcode(sBarcode, Type.Code93, true, 300, 150);
            var barcode = new Barcode(sBarcode, Type.Code93, true);
            return barcode.GetBase64Image();
        }
        catch
        {
            return default;
        }
    }
    #endregion

    [HttpPost]
    public async Task<IActionResult> AddTempItemTransfer([FromBody] TransferItemDetailViewModel transferItemData)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { result = false, message = "กรุณาตรวจสอบจำนวนสินค้า/บาร์โค้ดให้ถูกต้อง" });
            //return BadRequest(ModelState);
        }

        try
        {
            //Regex regex = new Regex("^[A-Za-z0-9]+$");
            //var dd = regex.Match(transferItemData.sbarcode);
            //if (!regex.Match(transferItemData.sbarcode).Success)
            //{
            //    return Json(new { result = false, message = "กรุณากดเปลี่ยนภาษาคีย์บอร์ดเป็นอังกฤษ" });
            //}
            
            if(transferItemData.nqty <= 0)
            {
                return Json(new { result = false, message = "กรุณาระบุจำนวนสินค้าไม่น้อยกว่า 0" });
            }
            //if (transferItemData.nqty < 0)
            //{
            //    throw new Exception("กรุณาระบุจำนวนไม่น้อยกว่า 0");
            //}

            //var getDataByItemCode = new TransferItemDetailViewModel();

            //Get Current List
            List<TransferItemDetailViewModel> tempTransferItemList = HttpContext.Session.GetDataFromSession<List<TransferItemDetailViewModel>>(_sessionTempTransferItemName);

            #region Update when Already added
            if (tempTransferItemList != null)
            {
                //Check is exist from temp
                TransferItemDetailViewModel existData = tempTransferItemList.FirstOrDefault(w => w.sbarcode == transferItemData.sbarcode);
                if (existData != null)
                {
                    //Update QTY
                    tempTransferItemList.Where(w => w.sbarcode == transferItemData.sbarcode).ForEach(e =>
                    {
                        e.nqty = e.nqty + transferItemData.nqty;
                    });
                }
                else
                {
                    //Add new if doesn't exist in temp list
                    int lastId = tempTransferItemList != null && tempTransferItemList.Count > 0 ? tempTransferItemList.Last().nseq : 0;
                    lastId++;
                    transferItemData.nseq = lastId;
                    MappingTransferItem(ref transferItemData);
                    tempTransferItemList.Add(transferItemData);
                }
            }
            else
            {
                tempTransferItemList = new List<TransferItemDetailViewModel>();
                //Add new get last seq
                int lastId = tempTransferItemList != null && tempTransferItemList.Count > 0 ? tempTransferItemList.Last().nseq : 0;
                lastId++;
                transferItemData.nseq = lastId;
                MappingTransferItem(ref transferItemData);
                tempTransferItemList.Add(transferItemData);
            }
            #endregion

            #region Validate Qty in Stock TMItem by barcode before response
            BaseResponse<GetItemByIDResponseDTO> resItem = await _itemAPI.GetItemByBarCodeV2Async(new GetItemByBarcodeQuery {itembarcode = transferItemData.sbarcode });
            if (!resItem.result)
            {
                return Json(new { result = false, message = $"{resItem.error.error.message} บาร์โค้ด {transferItemData.sbarcode}" });
            }

            if (resItem.data.qty < tempTransferItemList.FirstOrDefault(w => w.nitemid == resItem.data.itemid)?.nqty)
            {
                return Json(new { result = false, message = $"ไม่สามารภทำรายการได้, เนื่องจากจำนวนสต๊อกสินไม่เพียงพอ" });
            }
            #endregion

            HttpContext.Session.SetDataToSession(_sessionTempTransferItemName, tempTransferItemList);
            return Json(new { result = true, message = "เพิ่มสินค้าโอนสำเร็จ", amount = tempTransferItemList.Sum(w => w.totalprice) });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetTempItemTransfer()
    {
        try
        {
            List<TransferItemDetailViewModel> tempList = HttpContext.Session.GetDataFromSession<List<TransferItemDetailViewModel>>(_sessionTempTransferItemName);

            #region if list is null => create new list with 0 member
            if (tempList == null)
            {
                tempList = new List<TransferItemDetailViewModel>();
                HttpContext.Session.SetDataToSession(_sessionTempTransferItemName, tempList);
            }
            #endregion
            return Json(new { data = tempList.OrderBy(o => o.nseq).ToList() });
        }
        catch
        {
            return Json(new { data = new List<TransferItemDetailViewModel>() });
        }

    }

    [HttpPost]
    public JsonResult DeleteTempItemTransfer(int seq)
    {
        try
        {
            List<TransferItemDetailViewModel> tempTransferItemList = HttpContext.Session.GetDataFromSession<List<TransferItemDetailViewModel>>(_sessionTempTransferItemName);
            TransferItemDetailViewModel todo = tempTransferItemList?.FirstOrDefault(m => m.nseq == seq);
            if (todo == null)
            {
                throw new Exception("ไม่สามารถลบข้อมูลได้");
            }

            tempTransferItemList.Remove(todo);
            HttpContext.Session.SetDataToSession(_sessionTempTransferItemName, tempTransferItemList);
            return Json(new { result = true, message = "Delete success.", amount = tempTransferItemList.Sum(w => w.totalprice) });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    /// <summary>
    /// Validate Item before save
    /// </summary>
    /// <param name="transferItemObj"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ItemTransferBarcodeDataValidation(TransferItemViewModel transferItemObj)
    {
        try
        {
            #region Get data from temp
            List<TransferItemDetailViewModel> tempList = HttpContext.Session.GetDataFromSession<List<TransferItemDetailViewModel>>(_sessionTempTransferItemName);
            if (tempList == null || tempList?.Count() == 0)
            {
                return Json(new { result = false, msg = $"ขออภัย ข้อมูลสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!." });
            }
            #endregion

            return Json(new { result = true, msg = "ตรวจสอบข้อมูลถูกต้อง." });
        }
        catch (Exception ex)
        {
            return Json(new { result = true, msg = $"ขออภัย, มีบางอย่างผิดพลาด!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ItemTransferDataValidation(TransferItemViewModel transferItemObj)
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

    private void MappingTransferItem(ref TransferItemDetailViewModel transferItemData)
    {
        BaseResponse<List<GetItemListResponseDTO>> resItems = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetItemListResponseDTO>>>("ITEM_DATA");
        string itemBarcode = transferItemData.sbarcode;

        GetItemListResponseDTO existItem = resItems.data.Where(w => !string.IsNullOrEmpty(w.barcode)).FirstOrDefault(w => w.barcode.Trim().ToUpper() == itemBarcode.Trim().ToUpper());
        transferItemData.nitemid = existItem != null ? existItem.itemid : 0;
        transferItemData.sitemname = existItem != null ? existItem.name : null;
        transferItemData.price = existItem != null ? existItem.price : 0;
    }

    private async Task<BaseResponse<List<GetItemListResponseDTO>>> GetItemSessionDataAsync()
    {
        BaseResponse<List<GetItemListResponseDTO>> res = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetItemListResponseDTO>>>("ITEM_DATA");
        if (res != null)
        {
            return res;
        }
        res = await _itemAPI.GetItemListAsync();
        HttpContext.Session.SetDataToSession("ITEM_DATA", res);
        return res;
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

    private async Task<List<SelectListItem>> GetTransferSourcehBranchItemListAsync(int transferTypeID)
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

    private async Task<List<SelectListItem>> GetTransferDestinationBranchItemListAsync(int transferTypeID, int filterOutBranchID = 0)
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

    private async Task<List<SelectListItem>> GetItemTransferItemListByTransferType(int transferTypeID, int brnchID = 0)
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

    private async Task<List<SelectListItem>> PrepareSelectBranch()
    {

        BaseResponse<List<GetBranchResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();
        resBranch.data = base.UserProfile.roleid == (int)EnumModel.UserRole.Sale
            ? resBranch.data.Where(w => base.UserProfile.access_branch.Select(s => s.branchid).Contains(w.branchid)).ToList()
            : resBranch.data;
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }

    private async Task<List<SelectListItem>> PrepareSelectBrand()
    {
        BaseResponse<List<GetItemBrandListResponseDTO>> resBranch = await _itemBrandAPI.GetItemBrandListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.brandname, Value = s.brandid.ToString() }).ToList();
    }

    private async Task<List<SelectListItem>> PrepareSelectItemTransferType()
    {
        BaseResponse<List<GetTransferTypeListResponseDTO>> resItemTransaferTypeList = await _itemTransferAPI.GetItemTransferTypeAsync();
        return resItemTransaferTypeList.data.Select(s => new SelectListItem { Text = s.transfertypename, Value = s.transfertypeid.ToString() }).ToList();
    }

    private async Task<List<SelectListItem>> PrepareSelectItemTransferStatus()
    {
        BaseResponse<List<GetItemTransferStatusResponseDTO>> resTransferSttus = await _itemTransferAPI.GetItemTransferStatusAsync();
        return resTransferSttus.data.Select(s => new SelectListItem { Text = s.transferstatusname_th, Value = s.transferstatusid.ToString() }).ToList();
    }

    private async Task<List<SelectListItem>> PrepareSelectSubItemType()
    {
        BaseResponse<List<GetSubItemTypeResponseDTO>> resData = await _subItemTypeAPI.GetSubItemTypeListAsync();
        return resData.data.Select(s => new SelectListItem { Text = s.subitemcode, Value = s.subitemtypeid.ToString() }).ToList();
    }

    private async Task<List<SelectListItem>> PrepareSelectItemType()
    {
        BaseResponse<List<GetItemTypeListResponseDTO>> resData = await _itemTypeAPI.GetItemTypeListAsync();
        return resData.data.Select(s => new SelectListItem { Text = s.itemtypename, Value = s.itemtypeid.ToString() }).ToList();
    }

    //GetUnitOfMeasureListAsync
    private async Task<List<SelectListItem>> PrepareSelectUnitOfMeasure()
    {
        BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resData = await _itemUnitOfMeasureAPI.GetUnitOfMeasureListAsync();
        return resData.data.Select(s => new SelectListItem { Text = s.unitofmeasurename, Value = s.unitofmeasureid.ToString() }).ToList();
    }
}
