using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.CreateBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.DeleteBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.UpdateBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.Stock)]
public class ItemBrandController : BaseController
{
    private readonly IItemBrandAPI _itemBrandAPI;
    public ItemBrandController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemBrandAPI itemBrandAPI) : base(httpClientRequest, mapper, log)
    {
        _itemBrandAPI = itemBrandAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public async Task<IActionResult> Edit(int brandID)
    {
        var resBrand = await _itemBrandAPI.GetItemBrandByIDAsync(brandID);
        EditItemBrandViewModel brandViewModel = EditMappingBrand(resBrand.data);

        ViewBag.ActiveStatus = PrepareSelectActiveStatus();
        return View(brandViewModel); ;
    }

    [HttpPost]
    public async Task<IActionResult> EditBrand([FromBody] EditItemBrandViewModel editBrandObj)
    {
        try
        {
            UpdateBrandCommand updateBrandCommand = MappingUpdateBrandCommand(editBrandObj);
            BaseResponse<CommandResponse> resUpdateBrand = await _itemBrandAPI.UpdateItemBrandAsync(updateBrandCommand);
            if (resUpdateBrand.result)
            {
                return Json(new JsonViewModel { result = resUpdateBrand.result, message = resUpdateBrand.message });
            }
            return Json(new JsonViewModel { result = resUpdateBrand.result, message = resUpdateBrand.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBrand([FromBody] DeleteItemBrandViewModel deleteBrandObj)
    {
        try
        {
            BaseResponse<CommandResponse> resDeleteBrand = await _itemBrandAPI.DeleteItemBrandAsync(new DeleteBrandCommand
            {
                brandid = deleteBrandObj.brandid,
                updatedby = base.UserProfile.username
            });
            if (resDeleteBrand.result)
            {
                return Json(new JsonViewModel { result = resDeleteBrand.result, message = resDeleteBrand.message });
            }
            return Json(new JsonViewModel { result = resDeleteBrand.result, message = resDeleteBrand.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetItemBrands()
    {
        try
        {
            var res = await GetItemBrandListAsync();
            return Json(new { result = true, message = "สำเร็จ", data = res });
        }
        catch (Exception ex)
        {
            _log.Error($"{ex.Message}");
            return Json(new { data = new List<GetItemBrandListResponseDTO>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromBody] CreateItemBrandViewModel reqData)
    {
        CreateBrandCommand createBrandCommand = PrepareCreateBrandCommand(reqData);
        BaseResponse<CommandResponse> res = await _itemBrandAPI.CreateItemBrandAsync(createBrandCommand);
        return Json(new { result = res.result, message = res.result ? "บันทึกข้อมูลสำเร็จ." : res.error.error.message });
    }

    #region Private Method
    private CreateBrandCommand PrepareCreateBrandCommand(CreateItemBrandViewModel reqData)
    {
        return new CreateBrandCommand
        {
            brandname = reqData.brandname,
            brandshortname = reqData.brandshortname,
            createdby = base.UserProfile.username,
            createddate = DateTime.Now,
            description = reqData.description,
            isactive = true
        };
    }

    private async Task<List<GetItemBrandListResponseDTO>> GetItemBrandListAsync()
    {
        try
        {
            BaseResponse<List<GetItemBrandListResponseDTO>> res = await _itemBrandAPI.GetItemBrandListAsync();
            return res.data;
        }
        catch (Exception ex)
        {
            _log.Error($"[ERROR][GetItemBrandListAsync]:{ex.Message}");
            return new List<GetItemBrandListResponseDTO>();
        }
    }

    private EditItemBrandViewModel EditMappingBrand(GetItemBrandListResponseDTO data)
    {
        EditItemBrandViewModel brandViewModel = new EditItemBrandViewModel
        {
            brandid = data.brandid,
            brandname = data.brandname,
            brandshortname = data.brandshortname,
            description = data.description,
            isactive = data.isactive.ToString(),
        };
        return brandViewModel;
    }

    private UpdateBrandCommand MappingUpdateBrandCommand(EditItemBrandViewModel editItemBrandView)
    {
        return new UpdateBrandCommand
        {
            brandid = editItemBrandView.brandid,
            brandname = editItemBrandView.brandname,
            brandshortname = editItemBrandView.brandshortname,
            desription = editItemBrandView?.description,
            updatedby = base.UserProfile.username,
            isactive = editItemBrandView.isactive.ToBool()
        };
    }

    private List<SelectListItem> PrepareSelectActiveStatus()
    {
        var selectItems = new List<SelectListItem>();
        selectItems.Add(new SelectListItem { Text = "ใช้งาน", Value = "true" });
        selectItems.Add(new SelectListItem { Text = "ไม่ใช้งาน", Value = "false" });
        return selectItems;
    }
    #endregion

}
