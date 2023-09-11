using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Infrastructure.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using CYRetailIMS.ComponentService.Web.Models;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Logging;
using static CYRetailIMS.ComponentService.Web.Models.EnumModel;
using CYRetailIMS.Application.ExternalService.ItemTransferAPI;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Staff)]
public class ItemController : BaseController
{
    private readonly IItemAPI _itemAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemTypeAPI _itemTypeAPI;
    private readonly IItemUnitOfMeasureAPI _itemUnitOfMeasureAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IItemTransferAPI _itemTransferAPI;

    public ItemController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemAPI itemAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTypeAPI itemTypeAPI,
        IItemUnitOfMeasureAPI itemUnitOfMeasureAPI,
        IBranchAPI branchAPI, 
        IItemTransferAPI itemTransferAPI) : base(httpClientRequest, mapper, log)
    {
        _itemAPI = itemAPI;
        _itemTypeAPI = itemTypeAPI;
        _itemBrandAPI = itemBrandAPI;
        _itemUnitOfMeasureAPI = itemUnitOfMeasureAPI;
        _branchAPI = branchAPI;
        _itemTransferAPI = itemTransferAPI;
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
        ViewBag.BranchList = await PrepareSelectBranc();
        return View();
    }

    public IActionResult TransferHistory()
    {
        return View();
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
    public async Task<IActionResult> GetItemByID(string itemId)
    {
        try
        {
            BaseResponse<GetItemListResponseDTO> res = await _itemAPI.GetItemByIdAsync(Convert.ToInt32(itemId));
            if (res.result)
            {
                return Json(new { result = true, data = res.data, msg = "สำเร็จ" });
            }
            return Json(new { result = false, msg = res.error.error.message });
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
        //BaseResponse<CommandResponse> resCreateItem = await _httpClientRequest.HttpRequestToObject<CommandResponse, 
        //    CreateItemCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/create"), createItemCommand);
        BaseResponse<CommandResponse> resCreateItem = await _itemAPI.CreateItemAsync(createItemCommand);
        if (resCreateItem.result)
        {
            #region Set Profile
            //UserProfileViewModel userProfile = _mapper.Map<UserProfileViewModel>(resLogin.data);
            //base.UserProfile = userProfile;
            //var principal = CreatePrincipal(userProfile);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            #endregion

            return Json(new JsonViewModel { result = resCreateItem.result, message = resCreateItem.message });
        }

        return Json(new JsonViewModel { result = resCreateItem.result, message = resCreateItem.error.error.message });
    }

    [HttpPost]
    public async Task<IActionResult> EditItem([FromBody] EditItemViewModel editItemObj)
    {
        UpdateItemCommand updateItemCommand = UpdateItemCommand(editItemObj);
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
        DeleteItemCommand delItemCommand = new DeleteItemCommand { itemid = delItemObj.ItemID };
        BaseResponse<CommandResponse> resDelItem = await _itemAPI.DeleteItemAsync(delItemCommand);
        if (resDelItem.result)
        {
            return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.message });
        }
        return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.error.error.message });
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
        try
        {
            sourceList = await GetTransferSourcehItemListAsync(transferTypeID);
            destinationList = await GetTransferDestinationItemListAsync(transferTypeID);
            //Set Search Data
            //DestinationSearchData = destinationList;
        }
        catch (Exception ex)
        {
            return Json(new { result = false, msg = "ไม่สามารถดึงข้อมูลสาขาได้, กรุณาลองใหม่อีกครั้ง" });
        }
        return Json(new { result = true, data_source = sourceList, data_destination = destinationList,  msg = "สำเร็จ" });
    }

    public async Task<List<SelectListItem>> GetTransferSourcehItemListAsync(int transferTypeID)
    {
        List<SelectListItem> res = new List<SelectListItem>();
        try
        {
            //คลัง ไป สาขา
            if(transferTypeID == (int)TransferType.WTB)
            {
                res.Add(new SelectListItem
                {
                    Text = "คลังสินค้าสำนักงานใหญ่",
                    Value = "99",
                });
                
            }
            else
            {
                var resBranch = await _branchAPI.GetBranchListAsync();
                res = (from a in resBranch.data
                       select new SelectListItem
                       {
                           Text  = a.branchname,
                           Value = a.branchid
                       }).ToList();
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex.Message, ex);
        }
        return res;
    }

    public async Task<List<SelectListItem>> GetTransferDestinationItemListAsync(int transferTypeID)
    {
        List<SelectListItem> res = new List<SelectListItem>();
        try
        {
            //คลัง ไป สาขา
            if (transferTypeID == (int)TransferType.WTW)
            {
                res.Add(new SelectListItem
                {
                    Text = "คลังสินค้าสำนักงานใหญ่",
                    Value = "99",
                });

            }
            else
            {
                var resBranch = await _branchAPI.GetBranchListAsync();
                res = (from a in resBranch.data
                       select new SelectListItem
                       {
                           Text = a.branchname,
                           Value = a.branchid
                       }).ToList();
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex.Message, ex);
        }
        return res;
    }

    public async Task<List<SelectListItem>> PrepareSelectBranc()
    {
        var resBranch = await _branchAPI.GetBranchListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid }).ToList();
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
            createdby = base.UserProfile.rolename,
            isactive = bool.TryParse(itemViewModel.IsActive, out bool isactive) && isactive,
            //isactive = itemViewModel.IsActive,
        };
    }

    private UpdateItemCommand UpdateItemCommand(EditItemViewModel itemViewModel)
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
            discountpercent = itemViewModel.DiscountPercent,
            price = itemViewModel.Price,
            updatedby = base.UserProfile.rolename,
            isactive = bool.TryParse(itemViewModel.IsActive, out bool isactive) && isactive
            //isactive = itemViewModel.IsActive
        };
    }

    private EditItemViewModel EditItemMapping(GetItemListResponseDTO itemResponseDTO)
    {
        EditItemViewModel editItemViewModel = _mapper.Map<EditItemViewModel>(itemResponseDTO);
        return editItemViewModel;
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
            approvestatus = 1,
            isactive = true,
            items = itemsTransfer
        };
    }
    #endregion


}
