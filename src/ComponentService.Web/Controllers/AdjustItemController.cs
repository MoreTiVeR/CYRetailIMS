using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.ExternalService.AdjustItemAPI;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class AdjustItemController : BaseController
{
    private readonly IAdjustItemAPI _adjustItemAPI;

    public AdjustItemController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IAdjustItemAPI adjustItemAPI) : base(httpClientRequest, mapper, log)
    {
        _adjustItemAPI = adjustItemAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Adjust()
    {
        return View();
    }
}
