
using System.Globalization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace CYRetailIMS.ComponentService.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Regis Service Container
        builder.Services.AddWebComponentServices(builder.Configuration, builder.Environment.EnvironmentName);

        //Add Log4net
        builder.AddLogging(builder.Environment);

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

        #region Limit Request Body Size
        //app.Use(async (context, next) =>
        //{
        //    var httpMaxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();

        //    if (httpMaxRequestBodySizeFeature is not null)
        //    {
        //        httpMaxRequestBodySizeFeature.MaxRequestBodySize = 300_000_000;
        //    }
        //    await next(context);
        //});
        #endregion

        //app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        app.Run();
    }
}