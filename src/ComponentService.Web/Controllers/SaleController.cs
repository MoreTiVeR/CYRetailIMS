using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Staff)]
public class SaleController : BaseController
{
    public SaleController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log) 
        : base(httpClientRequest, mapper, log)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

	public IActionResult Items()
	{
		return View();
	}
}
