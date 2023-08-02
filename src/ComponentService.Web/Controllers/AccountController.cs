using CYRetailIMS.Application.Common.Models.UI;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Auth([FromForm] LoginViewModel loginViewModel)
    {
        return Json(new { success = true, message = "Login Success." });
    }
}
