
using System.Globalization;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Filters;
using CYRetailIMS.ComponentService.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;

namespace CYRetailIMS.ComponentService.Web;

public class Program
{
    private static IConfiguration _configuration;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        _configuration = builder.Configuration;

        // Add services to the container
        // Register the global exception filter
        builder.Services.AddMvc().AddRazorRuntimeCompilation();
        builder.Services.AddControllersWithViews(opt =>
        {
            opt.Filters.Add<GlobalExceptionFilter>();
        });
       
        builder.Services.Configure<ErrorViewModel>(_configuration.GetSection("ExceptionSettings"));

        builder.Services.AddSession(opts =>
        {
            // make the session cookie Essential
            //opts.Cookie.Name = "CY.Session";
            opts.Cookie.IsEssential = true; 
            opts.IdleTimeout = TimeSpan.FromMinutes(30);
            //opts.Cookie.HttpOnly = true;
            //opts.Cookie.Name = "AT.Session";
            //opts.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //opts.IdleTimeout = TimeSpan.FromMinutes(sessionTimeOut);

            //opts.IdleTimeout = TimeSpan.FromMinutes(10);
            //opts.Cookie.HttpOnly = true;
            //opts.Cookie.IsEssential = true; // make the session cookie Essential
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.AccessDeniedPath = "/AccessDenied";
            options.LoginPath = "/Login";
        });

        #region Service

        #endregion

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
        //if (!app.Environment.IsDevelopment())
        //{
        //    app.UseExceptionHandler("/Home/Error");
        //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //    app.UseHsts();
        //}
        if (app.Environment.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();
            app.UseStatusCodePagesWithReExecute("/Errors/CustomError");
            app.UseExceptionHandler("/Errors/CustomError");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseStatusCodePagesWithReExecute("/Errors/CustomError");
            app.UseExceptionHandler("/Errors/CustomError");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePages();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseSession();

        app.UseRouting();
        app.UseAuthorization();

        //app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

        app.Run();
    }
}