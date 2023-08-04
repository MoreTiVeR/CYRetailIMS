using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class ReportController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
