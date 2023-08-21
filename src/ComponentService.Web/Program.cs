
using System.Globalization;
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
        
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        //app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        app.Run();
    }
}