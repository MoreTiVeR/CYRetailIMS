using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class SaleController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
