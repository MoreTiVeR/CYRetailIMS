using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class StockController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
