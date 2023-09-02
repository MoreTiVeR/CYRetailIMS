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

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Staff)]
public class ItemController : BaseController
{
    private readonly IItemAPI _itemAPI;
    private readonly IItemBrandAPI _itemBrandAPI;
    private readonly IItemTypeAPI _itemTypeAPI;
    private readonly IItemUnitOfMeasureAPI _itemUnitOfMeasureAPI;

    public ItemController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemAPI itemAPI,
        IItemBrandAPI itemBrandAPI,
        IItemTypeAPI itemTypeAPI,
        IItemUnitOfMeasureAPI itemUnitOfMeasureAPI) : base(httpClientRequest, mapper, log)
    {
        _itemAPI = itemAPI;
        _itemTypeAPI = itemTypeAPI;
        _itemBrandAPI = itemBrandAPI;
        _itemUnitOfMeasureAPI = itemUnitOfMeasureAPI;
    }

    public async Task<IActionResult> Index()
    {
        //BaseResponse<List<GetItemListResponseDTO>> resItemList = await _httpClientRequest.HttpRequestToObject<List<GetItemListResponseDTO>, GetItemListQuery>(HttpMethod.Get,
        //	new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/getitemlist"), null);
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await _itemAPI.GetItemListAsync();

        //BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _httpClientRequest.HttpRequestToObject<List<GetItemTypeListResponseDTO>, GetItemTypeListQuery>(HttpMethod.Get,
        //new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtype/v1/getitemtypelist"), null);
        BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _itemTypeAPI.GetItemTypeListAsync();

        //BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _httpClientRequest.HttpRequestToObject<List<GetItemBrandListResponseDTO>, GetItemBrandListQuery>(HttpMethod.Get,
        //    new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/getitembrandlist"), null);
        BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _itemBrandAPI.GetItemBrandListAsync();

        ViewBag.ItemList = resItemList;
        ViewBag.ItemTypeList = resItemTypeList;
        ViewBag.ItemBrandList = resItemBrandList;
        return View();
    }

    public async Task<IActionResult> Create()
    {
        //BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _httpClientRequest.HttpRequestToObject<List<GetItemTypeListResponseDTO>, GetItemTypeListQuery>(HttpMethod.Get,
        //    new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtype/v1/getitemtypelist"), null);

        //BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _httpClientRequest.HttpRequestToObject<List<GetItemBrandListResponseDTO>, GetItemBrandListQuery>(HttpMethod.Get,
        //    new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/getitembrandlist"), null);

        //BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _httpClientRequest.HttpRequestToObject<List<GetUnitOfMeasureListResponseDTO>, GetUnitOfMeasureListQuery>(HttpMethod.Get,
        //    new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/unitofmeasure/v1/getunitofmeasure"), null);

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

    public IActionResult Transfer()
    {
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

    //[Route("item/getitems")]
    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        //var form = Request.Form.ToList();
        //#region Filter
        //string filterType = string.Empty;
        //string filterValue = string.Empty;
        //var frmSearch = form.Where(w => w.Key == "search[value]").FirstOrDefault(); //.Value[0];
        //string filter = frmSearch.Value[0].ToLower().Trim();
        //if (!string.IsNullOrEmpty(filter) && filter.Split("|").Count() > 1 && !string.IsNullOrEmpty(filter.Split("|")[1]))
        //{
        //    filterType = filter.Split("|")[0].Trim().ToLower();
        //    filterValue = filter.Split("|")[1].Trim();
        //}

        //if (filter.Contains("|") == false)
        //{
        //    filterType = "searchbox";
        //    filterValue = filter;
        //}
        //#endregion
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
    #endregion

    
}
