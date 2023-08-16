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

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Staff)]
public class ItemController : BaseController
{
	public ItemController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log) : base(httpClientRequest, mapper, log)
	{
	}

	public async Task<IActionResult> Index()
	{
		BaseResponse<List<GetItemListResponseDTO>> resItemList = await _httpClientRequest.HttpRequestToObject<List<GetItemListResponseDTO>, GetItemListQuery>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/getitemlist"), null);

		BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _httpClientRequest.HttpRequestToObject<List<GetItemTypeListResponseDTO>, GetItemTypeListQuery>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtype/v1/getitemtypelist"), null);

		BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _httpClientRequest.HttpRequestToObject<List<GetItemBrandListResponseDTO>, GetItemBrandListQuery>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/getitembrandlist"), null);

		ViewBag.ItemList = resItemList;
		ViewBag.ItemTypeList = resItemTypeList;
		ViewBag.ItemBrandList = resItemBrandList;
		return View();
	}

	public async Task<IActionResult> Create()
	{
		BaseResponse<List<GetItemTypeListResponseDTO>> resItemTypeList = await _httpClientRequest.HttpRequestToObject<List<GetItemTypeListResponseDTO>, GetItemTypeListQuery>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtype/v1/getitemtypelist"), null);

		BaseResponse<List<GetItemBrandListResponseDTO>> resItemBrandList = await _httpClientRequest.HttpRequestToObject<List<GetItemBrandListResponseDTO>, GetItemBrandListQuery>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/getitembrandlist"), null);

		BaseResponse<List<GetUnitOfMeasureListResponseDTO>> resUnitOfMeasureList = await _httpClientRequest.HttpRequestToObject<List<GetUnitOfMeasureListResponseDTO>, GetUnitOfMeasureListQuery>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/unitofmeasure/v1/getunitofmeasure"), null);

		ViewBag.ItemTypeList = resItemTypeList;
		ViewBag.ItemBrandList = resItemBrandList;
		ViewBag.ItemUOMList = resUnitOfMeasureList;
		return View();
	}

	public IActionResult Detail(int itemid)
	{
		return View();
	}

	public IActionResult Edit(int itemid)
	{
		return View();
	}

    [HttpPost]
	public async Task<IActionResult> AddItem([FromBody] AddItemViewModel addItemObj)
	{
		CreateItemCommand createItemCommand = CreateItemCommand(addItemObj);
        BaseResponse<CommandResponse> resCreateItem = await _httpClientRequest.HttpRequestToObject<CommandResponse,
			CreateItemCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/create"), createItemCommand);
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
        };
	}
    #endregion
}
