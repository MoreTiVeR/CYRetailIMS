using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class PermissionController : Controller
{
    public IActionResult AccessDenied()
    {
        return View();
    }
}
