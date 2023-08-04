using CYRetailIMS.Application.Common.Models.UI;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Authen([FromBody] LoginViewModel loginObj)
    {
        if(!loginObj.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            return Json(new { result = false, message = "Invalid UserName or Password." });
        }
        return Json(new { result = true, message = "Login Success." });
    }

}
