
using System.Globalization;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Filters;
using CYRetailIMS.ComponentService.Web.Models;
using CYRetailIMS.Infrastructure.Common.HttpClientRequest;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Localization;

namespace CYRetailIMS.ComponentService.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Regis Service Container
        builder.Services.AddWebComponentServices(builder.Configuration, builder.Environment.EnvironmentName);

        //Application Builder
        var app = builder.Build();
        #region Configure the Localization middleware
        CultureInfo ci = new CultureInfo("en-US");
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(ci),
            SupportedCultures = new List<CultureInfo> { ci },
            SupportedUICultures = new List<CultureInfo> { ci }
        });
        #endregion

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();
            //app.UseStatusCodePagesWithRedirects("Error/ErrorPage?statusCode={0}");
            app.UseExceptionHandler("/Errors/CustomError");
            app.UseStatusCodePagesWithReExecute("/Errors/CustomError/{0}");
            app.UseHsts();
        }
        else
        {
            //app.UseStatusCodePagesWithRedirects("Error/ErrorPage?statusCode={0}");
            app.UseExceptionHandler("/Errors/CustomError");
            app.UseStatusCodePagesWithReExecute("/Errors/CustomError/{0}");
            app.UseHsts();
        }

        //app.UseStatusCodePages();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.None,
            MinimumSameSitePolicy = SameSiteMode.None,
            HttpOnly = HttpOnlyPolicy.None
        });
        app.UseSession();
        app.UseRouting();
        app.UseAuthorization();

        //app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        app.Run();
    }

    //public static void RegisterBundles(BundleCollection bundles)
    //{
    //    bundles.Add(new ScriptBundle("~/bundles/js").Include(
    //      "~/Scripts/bootstrap.js",
    //      "~/Scripts/jquery-3.3.1.js"));
    //}
}