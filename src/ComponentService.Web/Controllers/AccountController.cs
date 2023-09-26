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
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class AccountController : BaseController
{
    private readonly IAccountAPI _accountAPI;
    private readonly IEmployeeAPI _employeeAPI;
    private readonly IBranchAPI _branchAPI;
    public AccountController(IHttpClientRequest httpClientRequest, IMapper mapper,
        ILog4NetLogger log4NetLogger,
        IAccountAPI accountAPI,
        IEmployeeAPI employeeAPI,
        IBranchAPI branchAPI)
        : base(httpClientRequest, mapper, log4NetLogger)
    {
        _accountAPI = accountAPI;
        _employeeAPI = employeeAPI;
        _branchAPI = branchAPI;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult LoginV2()
    {
        return View();
    }

    public async Task<IActionResult> Register()
    {
        BaseResponse<List<GetBranchListResponseDTO>> resBrachList = await _branchAPI.GetBranchListAsync();
        ViewBag.BranchList = resBrachList;
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
                return Json(new JsonViewModel { result = resLogin.result, message = $"ล็อกอินสำเร็จ, ยินดีต้อนรับ {userProfile.username}", url = Url.Action("Index", "Home") });
            }
            return Json(new JsonViewModel { result = resLogin.result, message = resLogin.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new JsonViewModel { result = false, message = $"ไม่สามารถเข้าสู่ระบบได้ เนื่องจากเกิดข้อผิดพลาด, กรุณาลองใหม่อีกครั้ง <br>{ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] AccountRegisterViewModel regisData)
    {
        try
        {
            CreateEmployeeCommand createAccountCommand = CreateEmployeeCommand(regisData);
            BaseResponse<CommandResponse> res = await _employeeAPI.CreateEmployeeAsync(createAccountCommand);
            return Json(new { result = res.result, message = res.result ? "ลงทะเบียนสมาชิกสำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    private CreateEmployeeCommand CreateEmployeeCommand(AccountRegisterViewModel regisData)
    {
        return new CreateEmployeeCommand
        {
            departmentid = 3, //Sale & Marketing
            firstname = regisData.firstname,
            lastname = regisData.lastname,
            email = regisData.email,
            salary = 10000,
            startworkingdate = DateTime.Now,
            creadeddate = DateTime.Now,
            createdby = "SYSTEM",
        };
    }

}
