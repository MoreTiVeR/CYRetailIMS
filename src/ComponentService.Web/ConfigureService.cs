using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Filters;
using CYRetailIMS.ComponentService.Web.Common.Mappings.Account;
using CYRetailIMS.ComponentService.Web.Models;
using CYRetailIMS.Infrastructure.Common.HttpClientRequest;
using CYRetailIMS.Infrastructure.Common.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CYRetailIMS.ComponentService.Web;

public static class ConfigureService
{
    public static IServiceCollection AddWebComponentServices(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        services.AddMvc().AddRazorRuntimeCompilation();
        services.AddControllersWithViews(opt =>
        {
            opt.Filters.Add<GlobalExceptionFilter>();
        });

        services.Configure<ErrorViewModel>(configuration.GetSection("ExceptionSettings"));

        services.AddSession(opts =>
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

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.AccessDeniedPath = "/AccessDenied";
            options.LoginPath = "/Login";
        });

        #region Auto Mapper Configurations
        var mappingConfig = new MapperConfiguration(mc =>
        {
            #region WEB
            mc.AddProfile<AcountMappingProfile>();
            #endregion
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
        #endregion

        #region Service
        services.AddHttpContextAccessor();
        services.AddHttpClient<IHttpClientRequest, HttpClientRequest>();

        services.AddSingleton<CYRetailIMS.Application.Common.Interfaces.ILog4NetLogger, CYRetailIMS.Infrastructure.Common.Logging.Log4NetLogger>();
        services.AddTransient<IDateTimeProvider, DateTimeService>();
        #endregion

        return services;
    }
}
