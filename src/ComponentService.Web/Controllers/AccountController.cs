using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class AccountController : Controller
{
    private readonly IHttpClientRequest _httpClientRequest;
    public AccountController(IHttpClientRequest httpClientRequest)
    {
        _httpClientRequest = httpClientRequest;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Authen([FromBody] LoginViewModel loginObj)
    {
       var resLogin = await _httpClientRequest.HttpRequestToObject<UserProfileResponseDTO,
                    LoginQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}api/v1/account/v1/login"),
                    new LoginQuery { UserName = loginObj.UserName, Password = loginObj.Password });
        if (!resLogin.Result)
        {
            //return Json(new { result = false, message = "Invalid UserName or Password." });
            return Json(new JsonViewModel { Message = resLogin.Error.Error.Message });
        }
        return Json(new JsonViewModel { Result = true, Message = "Login Success", RedirectUrl = Url.Action("Index", "Home") });
        //return Json(new { result = true, message = "Success", redirect_url = Url.Action("Index", "Home"), url = Url.Action("Index", "Home") });
    }

}
