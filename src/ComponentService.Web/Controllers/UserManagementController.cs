using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class UserManagementController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
