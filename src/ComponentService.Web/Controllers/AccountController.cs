using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class AccountController : BaseController
{
    public AccountController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log4NetLogger) 
        : base(httpClientRequest, mapper, log4NetLogger)
    {
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    public async Task<IActionResult> Authen([FromBody] LoginViewModel loginObj)
    {
        BaseResponse<UserProfileResponseDTO> resLogin = await _httpClientRequest.HttpRequestToObject<UserProfileResponseDTO,
                    LoginQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}api/v1/account/v1/login"),
                    new LoginQuery { username = loginObj.UserName, password = loginObj.Password });
        if (resLogin.result)
        {
            #region Set Profile
            UserProfileViewModel userProfile = _mapper.Map<UserProfileViewModel>(resLogin.data);
            base.UserProfile = userProfile;
            var principal = CreatePrincipal(userProfile);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            #endregion

            return Json(new JsonViewModel { result = true, message = "Login Success", url = Url.Action("Index", "Home") });
        }

        return Json(new JsonViewModel { result = resLogin.result, message = resLogin.error.error.message });
    }

}
