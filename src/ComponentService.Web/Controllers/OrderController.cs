using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class OrderController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
