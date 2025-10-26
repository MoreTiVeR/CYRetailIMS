using System.Security.Claims;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Infrastructure.Common.Extensions;
using CYRetailIMS.Infrastructure.Common.HttpClientRequest;
using log4net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class BaseController : Controller
{
    protected readonly IHttpClientRequest _httpClientRequest;
    protected readonly IMapper _mapper;
    protected ILog4NetLogger _log;
    public UserProfileViewModel UserProfile
    {
        get => HttpContext.Session.GetDataFromSession<UserProfileViewModel>("userprofile");
        set => HttpContext.Session.SetDataToSession("userprofile", value);
    }

    public BaseController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log)
    {
        _httpClientRequest = httpClientRequest;
        _mapper = mapper;
        _log = log;
	}

    protected void InitialData()
    {
		ViewData["firstname"] = UserProfile.firstname;
		ViewData["lastname"] = UserProfile.lastname;
		ViewData["menu"] = UserProfile.access_menu;

		ViewBag.Menus = UserProfile.access_menu;
	}

	protected ClaimsPrincipal CreatePrincipal(UserProfileViewModel result)
    {
        var id = new ClaimsIdentity("Cookies");
        id.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.userid.ToString()));
        id.AddClaim(new Claim(ClaimTypes.Name, result.username));
        id.AddClaim(new Claim(ClaimTypes.Role, result.rolename));
        return new ClaimsPrincipal(id);
        //var claims = new List<Claim>
        //    {
        //        new Claim("UserId", result.userid.ToString()),
        //        new Claim("UserName", result.username),
        //        new Claim("RoleName", result.rolename),
        //        new Claim("AccessBranch", result.access_branch.ToJson()),
        //        new Claim("AccessMenu", result.access_menu.ToJson())
        //        //new Claim("CanRead", "CanRead"),
        //        //new Claim("CanWrite", "CanWrite")
        //    };

        //ClaimsPrincipal principal = new ClaimsPrincipal();
        //principal.AddIdentity(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        //return principal;
    }

}
