using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.Stock)]
public class InventoryController : BaseController
{
    public InventoryController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log) : base(httpClientRequest, mapper, log)
    {
    }

    public IActionResult Index()
    {
        return View();
    }
}
