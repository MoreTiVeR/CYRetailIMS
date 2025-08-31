using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class SubItemTypeController : BaseController
{
    public SubItemTypeController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log) : base(httpClientRequest, mapper, log)
    {
    }

    public IActionResult Index()
    {
        return View();
    }
}
