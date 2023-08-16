using System.Diagnostics;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.ComponentService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Staff, RoleName.AccountingOfficer, RoleName.Manager)]
public class HomeController : BaseController
{
    public HomeController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log) 
        : base(httpClientRequest, mapper, log)
    {
    }

    public IActionResult Index()
    {
        //base.InitialData();
		return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
