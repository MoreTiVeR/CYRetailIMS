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
using System.Collections.Generic;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.Stock)]
public class ItemController : BaseController
{
    private readonly IItemAPI _itemAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemTypeAPI _itemTypeAPI;
    private readonly IItemUnitOfMeasureAPI _itemUnitOfMeasureAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IItemTransferAPI _itemTransferAPI;
    private readonly IItemInBranchAPI _itemInBranchAPI;

    public ItemController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemAPI itemAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTypeAPI itemTypeAPI,
        IItemUnitOfMeasureAPI itemUnitOfMeasureAPI,
        IBranchAPI branchAPI,
        IItemTransferAPI itemTransferAPI,
        IItemInBranchAPI itemInBranchAPI) : base(httpClientRequest, mapper, log)
    {
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
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await _itemAPI.GetItemListAsync();
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();
        ViewBag.ItemList = resItemList;
        ViewBag.ItemTypeList = resItemTypeList;
        ViewBag.ItemBrandList = resItemBrandList;
        return View();
    }

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

        BaseResponse<List<GetBranchListResponseDTO>> resBrachList = await _branchAPI.GetBranchListAsync();
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
        CreateItemCommand createItemCommand = CreateItemCommand(addItemObj);
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
        UpdateItemCommand updateItemCommand = PrepareUpdateItemCommand(editItemObj);
        BaseResponse<CommandResponse> resUpdateItem = await _itemAPI.UpdateItemAsync(updateItemCommand);
        if (resUpdateItem.result)
        {
            return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.message });
        }
        return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.error.error.message });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteItem([FromBody] DeleteItemViewModel delItemObj)
    {
        DeleteItemCommand delItemCommand = new DeleteItemCommand { itemid = delItemObj.itemid };
        BaseResponse<CommandResponse> resDelItem = await _itemAPI.DeleteItemAsync(delItemCommand);
        if (resDelItem.result)
        {
            return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.message });
        }
        return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.error.error.message });
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

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await _itemAPI.GetItemListAsync();
        return Json(new { data = resItemList.data });
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

    public async Task<List<SelectListItem>> GetTransferSourcehBranchItemListAsync(int transferTypeID)
    {
        List<SelectListItem> res = new List<SelectListItem>();
        try
        {
            BaseResponse<List<GetBranchListResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();
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
            BaseResponse<List<GetBranchListResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();
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
                //res.Add(new SelectListItem
                //{
                //    Text = "คลังสินค้าสำนักงานใหญ่",
                //    Value = "99",
                //});
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

    #region Private Method
    private CreateItemCommand CreateItemCommand(AddItemViewModel itemViewModel)
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
            shortname = !string.IsNullOrEmpty(itemViewModel.ShortName) ? itemViewModel.ShortName : itemViewModel.Name,
            itemimageurl = !string.IsNullOrEmpty(itemViewModel.ItemImageUrl) ? itemViewModel.ItemImageUrl : "../assets/img/product/noimage.png",
            price = itemViewModel.Price,
            qty = itemViewModel.Qty,
            notifyminqty = itemViewModel.NotifyMinQty,
            createdby = base.UserProfile.rolename,
            isactive = bool.TryParse(itemViewModel.IsActive, out bool isactive) && isactive,
        };
    }

    private UpdateItemCommand PrepareUpdateItemCommand(EditItemViewModel itemViewModel)
    {
        return new UpdateItemCommand
        {
            itemid = itemViewModel.ItemID,
            name = itemViewModel.Name,
            barcode = itemViewModel.BarCode,
            description = itemViewModel.Description,
            shortname = !string.IsNullOrEmpty(itemViewModel.ShortName) ? itemViewModel.ShortName : itemViewModel.Name,
            itemimageurl = !string.IsNullOrEmpty(itemViewModel.ItemImageUrl) ? itemViewModel.ItemImageUrl : "../assets/img/product/noimage.png",
            qty = itemViewModel.Qty,
            notifyqty = itemViewModel.NotifyMinQty,
            discountpercent = itemViewModel.DiscountPercent,
            price = itemViewModel.Price,
            updatedby = base.UserProfile.rolename,
            isactive = bool.TryParse(itemViewModel.IsActive, out bool isactive) && isactive
        };
    }

    private EditItemViewModel EditItemMapping(GetItemListResponseDTO itemResponseDTO)
    {
        EditItemViewModel editItemViewModel = _mapper.Map<EditItemViewModel>(itemResponseDTO);
        return editItemViewModel;
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
