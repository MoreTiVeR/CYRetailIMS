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
using CYRetailIMS.Application.ExternalService.AccountAPI;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class AccountController : BaseController
{
    private readonly IAccountAPI _accountAPI;
    public AccountController(IHttpClientRequest httpClientRequest, IMapper mapper, 
        ILog4NetLogger log4NetLogger, 
        IAccountAPI accountAPI)
        : base(httpClientRequest, mapper, log4NetLogger)
    {
        _accountAPI = accountAPI;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    public async Task<JsonResult> Authen([FromBody] LoginViewModel loginObj)
    {
        BaseResponse<UserProfileResponseDTO> resLogin = null;
        try
        {
            resLogin = await _accountAPI.LoginAsync(new LoginQuery { username = loginObj.UserName, password = loginObj.Password });
            if (resLogin.result)
            {
                #region Set Profile
                UserProfileViewModel userProfile = _mapper.Map<UserProfileViewModel>(resLogin.data);
                #region Order SubMenu
                //userProfile.access_menu = userProfile.access_menu.Select(e =>
                //{
                //    e.submenulist = e.submenulist.OrderBy(s => s.seq).ToList();
                //    return e;
                //}).ToList();
                #endregion
                base.UserProfile = userProfile;
                var principal = CreatePrincipal(userProfile);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                #endregion
                return Json(new JsonViewModel { result = resLogin.result, message = "เข้าสู่ระบบสำเร็จ", url = Url.Action("Index", "Home") });
            }
            return Json(new JsonViewModel { result = resLogin.result, message = resLogin.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"ไม่สามารถเข้าสู่ระบบได้ เนื่องจากเกิดข้อผิดพลาด, กรุณาลองใหม่อีกครั้ง <br>{ex.Message}" });
        }
    }

}
