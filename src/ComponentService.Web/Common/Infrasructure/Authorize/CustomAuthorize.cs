using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;

/// <summary>
/// Ref : https://www.codeproject.com/Articles/5247609/ASP-NET-CORE-Token-Authentication-and-Authorizat-2
/// </summary>
#region Authorize
public class CustomAuthorize : TypeFilterAttribute
{
	public static class RoleName
	{
		public const string Admin = "Admin";
		public const string Sale = "Sale";
		public const string Stock = "Stock";
		public const string AccountingOfficer = "Accounting Officer";
		public const string SaleArea = "Sale Area";
        public const string Audit = "Audit";
    }

	public CustomAuthorize(params string[] claim) : base(typeof(AuthorizeFilter))
	{
		Arguments = new object[] { claim };
	}

	public class AuthorizeFilter : IAuthorizationFilter
	{
		readonly string[] _claim;

		public AuthorizeFilter(params string[] claim)
		{
			_claim = claim;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
            var user = context.HttpContext.User;
            var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
			var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;

			// Retrieve the user's role and requested resource URL
			//string userRole = /* Get the user's role */;
			//string resourceUrl = context.HttpContext.Request.Path;
			//Query the database for matching authorization rules
			//using (var dbContext = new ApplicationDbContext())
			//{
			//    bool isAuthorized = dbContext.AuthorizationRules
			//        .Any(rule => rule.Role == userRole &&
			//                     rule.ResourceUrl == resourceUrl &&
			//                     rule.Permission == "Allow");

			//    return isAuthorized;
			//}

			if (IsAuthenticated)
			{
				//bool flagClaim = false;
				//foreach (var item in _claim)
				//{
				//	if (context.HttpContext.User.HasClaim("RoleName", item))
				//		flagClaim = true;
				//}

                //if (!flagClaim || context.HttpContext.Session.GetString("userprofile") is null)
                //{
                //    context.Result = new RedirectResult("~/Permission/AccessDenied");
                //}


                #region Testing
                //var requestPath = context.HttpContext.Request.Path.Value;
                //RouteValueDictionary routeValues = context.HttpContext.Request.RouteValues;
                //string controllerName = routeValues["controller"].ToString();
                //string actionName = routeValues["action"].ToString();

                //List<GetMenuByRoleIDResponseDTO> accessMenu = JsonConvert.DeserializeObject<List<GetMenuByRoleIDResponseDTO>>(claimsIndentity.Claims.FirstOrDefault(w => w.Type == "AccessMenu").Value);
                //var menu = accessMenu.SelectMany(s => s.submenulist).FirstOrDefault(w => w.cms_actionname == actionName && w.cms_controllername == controllerName);
                //if (!accessMenu.SelectMany(s => s.submenulist).Any(a => a.cms_controllername == controllerName && a.cms_actionname == actionName))
                //{
                //	flagClaim = false;
                //}
                #endregion


                var inRole = _claim.Any(r => user.IsInRole(r) || user.HasClaim(ClaimTypes.Role, r) || user.HasClaim("RoleName", r));
                if (!inRole || context.HttpContext.Session.GetString("userprofile") is null)
                    context.Result = new RedirectResult("~/Permission/AccessDenied");
            }
			else
			{
				context.Result = new RedirectResult("~/Permission/AccessDenied");
			}
			return;
		}
	}



	#region Unauthorized 
	public class UnAuthorizedAttribute : TypeFilterAttribute
	{
		public UnAuthorizedAttribute() : base(typeof(UnauthorizedFilter))
		{
			//Empty constructor
		}
	}
	public class UnauthorizedFilter : IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			bool IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
			if (!IsAuthenticated)
			{
				context.Result = new RedirectResult("~/Home/Index");
			}
		}
	}
	#endregion

}
#endregion