using System.Configuration;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Mappings.UI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Filters;
using CYRetailIMS.ComponentService.Web.Common.Mappings.Account;
using CYRetailIMS.ComponentService.Web.Models;
using CYRetailIMS.Infrastructure.Common.HttpClientRequest;
using CYRetailIMS.Infrastructure.Common.Service;
using CYRetailIMS.Infrastructure.ExternalService.ItemAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemBrand;
using CYRetailIMS.Infrastructure.ExternalService.ItemTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemUnitOfMeasureAPI;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CYRetailIMS.ComponentService.Web;

public static class ConfigureService
{
    public static IServiceCollection AddWebComponentServices(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        #region Session Timeout
        int sessionTimeout = configuration.GetSection("Appsettings")["SESSION_TIMEOUT"] != null ? int.Parse(configuration.GetSection("Appsettings")["SESSION_TIMEOUT"]) : 60;
        #endregion

        services.AddMvc().AddRazorRuntimeCompilation();
        services.AddControllersWithViews(opt =>
        {
            opt.Filters.Add<GlobalExceptionFilter>();
        });

        services.Configure<ErrorViewModel>(configuration.GetSection("ExceptionSettings"));

        services.AddSession(opts =>
        {
            opts.Cookie.Name = "CY.Session";
            opts.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);//You can set Time
            opts.Cookie.IsEssential = true;
        });

        services.AddDataProtection();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.AccessDeniedPath = "/AccessDenied";
            options.LoginPath = "/Login";
        });

        #region Auto Mapper Configurations
        var mappingConfig = new MapperConfiguration(mc =>
        {
            #region UI/WEB
            mc.AddProfile<AcountMappingProfile>();
            mc.AddProfile<ItemMappingProfile>();
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

        #region External Service
        services.AddScoped<IItemAPI, ItemAPI>();
        services.AddScoped<IItemTypeAPI, ItemTypeAPI>();
        services.AddScoped<IItemBrandAPI, ItemBrandAPI>();
        services.AddScoped<IItemUnitOfMeasureAPI, ItemUnitOfMeasureAPI>();
        #endregion

        return services;
    }
}
