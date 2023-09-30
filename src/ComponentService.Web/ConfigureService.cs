using System.Configuration;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Mappings.UI.Account;
using CYRetailIMS.Application.Common.Mappings.UI.Employee;
using CYRetailIMS.Application.Common.Mappings.UI.Item;
using CYRetailIMS.Application.Common.Mappings.UI.User;
using CYRetailIMS.Application.ExternalService.AccountAPI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.DepartmentAPI;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.ExternalService.ItemTransferAPI;
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Application.ExternalService.Report;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.ExternalService.UserAPI;
using CYRetailIMS.Application.ExternalService.UserRoleAPI;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Filters;
using CYRetailIMS.ComponentService.Web.Models;
using CYRetailIMS.Infrastructure.Common.HttpClientRequest;
using CYRetailIMS.Infrastructure.Common.Service;
using CYRetailIMS.Infrastructure.ExternalService.AccountAPI;
using CYRetailIMS.Infrastructure.ExternalService.BranchAPI;
using CYRetailIMS.Infrastructure.ExternalService.DepartmentAPI;
using CYRetailIMS.Infrastructure.ExternalService.EmployeeAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemBrand;
using CYRetailIMS.Infrastructure.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemTransferAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Infrastructure.ExternalService.ReportAPI;
using CYRetailIMS.Infrastructure.ExternalService.TransactionAPI;
using CYRetailIMS.Infrastructure.ExternalService.UserAPI;
using CYRetailIMS.Infrastructure.ExternalService.UserRoleAPI;
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

            #region Employee
            mc.AddProfile<EmployeeMappingProfile>();
            #endregion

            #region User
            mc.AddProfile<UserMappingProfile>();
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
        services.AddScoped<IAccountAPI, AccountAPI>();
        services.AddScoped<IEmployeeAPI, EmployeeAPI>();
        services.AddScoped<IItemAPI, ItemAPI>();
        services.AddScoped<IItemTypeAPI, ItemTypeAPI>();
        services.AddScoped<IItemBrandAPI, ItemBrandAPI>();
        services.AddScoped<IItemUnitOfMeasureAPI, ItemUnitOfMeasureAPI>();
        services.AddScoped<IItemInBranchAPI, ItemInBranchAPI>();
        services.AddScoped<ITransactionAPI, TransactionAPI>();
        services.AddScoped<IBranchAPI, BranchAPI>();
        services.AddScoped<IItemTransferAPI, ItemTransferAPI>();
        services.AddScoped<IDepartmentAPI, DepartmentAPI>();
        services.AddScoped<IUserAPI, UserAPI>();
        services.AddScoped<IUserRoleAPI, UserRoleAPI>();
        services.AddScoped<IReportAPI, ReportAPI>();
        #endregion

        return services;
    }
}
