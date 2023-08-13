using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public const string Staff = "Staff";
        public const string Manager = "Manager";
        public const string AccountingOfficer = "Accounting";
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
            var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (IsAuthenticated)
            {
                bool flagClaim = false;
                foreach (var item in _claim)
                {
                    if (context.HttpContext.User.HasClaim("RoleName", item))
                        flagClaim = true;
                }

                if (!flagClaim)
                {
                    context.Result = new RedirectResult("~/Permission/AccessDenied");
                }
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