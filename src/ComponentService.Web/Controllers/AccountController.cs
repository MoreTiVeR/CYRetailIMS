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
    public async Task<IActionResult> Authen([FromBody] LoginViewModel loginObj)
    {
        Thread.Sleep(1000);
        if(!loginObj.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            //return Json(new { result = false, message = "Invalid UserName or Password." });
            return Json(new JsonViewModel { Message = "Invalid UserName or Password" });
        }
        return Json(new JsonViewModel { Result = true, Message = "Login Success", RedirectUrl = Url.Action("Index", "Home") });
        //return Json(new { result = true, message = "Success", redirect_url = Url.Action("Index", "Home"), url = Url.Action("Index", "Home") });
    }

}
