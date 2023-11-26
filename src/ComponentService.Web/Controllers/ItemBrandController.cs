using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.CreateBrand.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromBody] CreateItemBrandViewModel reqData)
    {
        CreateBrandCommand createBrandCommand = PrepareCreateBrandCommand(reqData);
        BaseResponse<CommandResponse> res = await _itemBrandAPI.CreateItemBrandAsync(createBrandCommand);
        return Json(new { result = res.result, message = res.result ? "บันทึกข้อมูลสำเร็จ." : res.error.error.message });
    }

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
}
