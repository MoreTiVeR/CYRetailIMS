using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using AutoMapper;
using CYRetailIMS.Application.Common.Confiuration;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Mappings.UI.Account;
using CYRetailIMS.Application.Common.Mappings.UI.Employee;
using CYRetailIMS.Application.Common.Mappings.UI.Item;
using CYRetailIMS.Application.Common.Mappings.UI.MoneyTransfer;
using CYRetailIMS.Application.Common.Mappings.UI.Report;
using CYRetailIMS.Application.Common.Mappings.UI.Supplier;
using CYRetailIMS.Application.Common.Mappings.UI.Transaction;
using CYRetailIMS.Application.Common.Mappings.UI.User;
using CYRetailIMS.Application.ExternalService.AccountAPI;
using CYRetailIMS.Application.ExternalService.AdjustItemAPI;
using CYRetailIMS.Application.ExternalService.AdjustItemTypeAPI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ChartAPI;
using CYRetailIMS.Application.ExternalService.CountStockAPI;
using CYRetailIMS.Application.ExternalService.CurrencyAPI;
using CYRetailIMS.Application.ExternalService.DepartmentAPI;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.ExternalService.ExcelAPI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.ExternalService.ItemTransferAPI;
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Application.ExternalService.MoneyTransferAPI;
using CYRetailIMS.Application.ExternalService.MoneyTransferSlipAPI;
using CYRetailIMS.Application.ExternalService.PaymentTypeAPI;
using CYRetailIMS.Application.ExternalService.PurchaseOrderAPI;
using CYRetailIMS.Application.ExternalService.PurchaseTypeAPI;
using CYRetailIMS.Application.ExternalService.ReportAPI;
using CYRetailIMS.Application.ExternalService.ShipmentTypeAPI;
using CYRetailIMS.Application.ExternalService.SubItemTypeAPI;
using CYRetailIMS.Application.ExternalService.SupplierAPI;
using CYRetailIMS.Application.ExternalService.SupplierContactTypeAPI;
using CYRetailIMS.Application.ExternalService.SupplierTypeAPI;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.ExternalService.UserAPI;
using CYRetailIMS.Application.ExternalService.UserRoleAPI;
using CYRetailIMS.Application.ExternalService.WarehouseAPI;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Filters;
using CYRetailIMS.ComponentService.Web.Models;
using CYRetailIMS.Infrastructure.Common.Configuration;
using CYRetailIMS.Infrastructure.Common.HttpClientRequest;
using CYRetailIMS.Infrastructure.Common.Service;
using CYRetailIMS.Infrastructure.ExternalService;
using CYRetailIMS.Infrastructure.ExternalService.AccountAPI;
using CYRetailIMS.Infrastructure.ExternalService.AdjustItemAPI;
using CYRetailIMS.Infrastructure.ExternalService.AdjustItemTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.BranchAPI;
using CYRetailIMS.Infrastructure.ExternalService.ChartAPI;
using CYRetailIMS.Infrastructure.ExternalService.CountStockAPI;
using CYRetailIMS.Infrastructure.ExternalService.CurrencyAPI;
using CYRetailIMS.Infrastructure.ExternalService.DepartmentAPI;
using CYRetailIMS.Infrastructure.ExternalService.EmployeeAPI;
using CYRetailIMS.Infrastructure.ExternalService.ExcelAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemBrand;
using CYRetailIMS.Infrastructure.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemTransferAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Infrastructure.ExternalService.MoneyTransferAPI;
using CYRetailIMS.Infrastructure.ExternalService.MoneyTransferSlipAPI;
using CYRetailIMS.Infrastructure.ExternalService.PaymentTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.PurchaseOrderAPI;
using CYRetailIMS.Infrastructure.ExternalService.PurchaseTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.ReportAPI;
using CYRetailIMS.Infrastructure.ExternalService.ShipmentTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.SubItemTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.SupplierAPI;
using CYRetailIMS.Infrastructure.ExternalService.SupplierContactTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.SupplierTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.TransactionAPI;
using CYRetailIMS.Infrastructure.ExternalService.UserAPI;
using CYRetailIMS.Infrastructure.ExternalService.UserRoleAPI;
using CYRetailIMS.Infrastructure.ExternalService.WarehouseAPI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;

namespace CYRetailIMS.ComponentService.Web;

public static class ConfigureService
{
    public static IServiceCollection AddWebComponentServices(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        #region Session Timeout
        int sessionTimeout = configuration.GetSection("Appsettings")["SESSION_TIMEOUT"] != null ? int.Parse(configuration.GetSection("Appsettings")["SESSION_TIMEOUT"]) : 60;
        #endregion

        #region Add Razor compile at runtime
        services.AddRazorPages().AddRazorRuntimeCompilation();
        #endregion

        services.AddControllersWithViews(opt =>
        {
            opt.Filters.Add<GlobalExceptionFilter>();
        });

        services.Configure<ErrorViewModel>(configuration.GetSection("ExceptionSettings"));

        #region Limit Max Request
        //Max Request For application running on Kestrel
        //services.Configure<KestrelServerOptions>(options =>
        //{
        //    options.Limits.MaxRequestBodySize = int.MaxValue;
        //});

        //Max Request For application running on IIS
        services.Configure<IISServerOptions>(options =>
        {
            options.MaxRequestBodySize = int.MaxValue;
        });

        //Form's MultipartBodyLengthLimit
        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = int.MaxValue;
            options.MultipartHeadersLengthLimit = int.MaxValue;
        });
        #endregion

        #region Allow edit razor and redendering
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
        services.AddMvc().AddRazorRuntimeCompilation();
        #endregion

        #region Cookie & Session
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.Cookie.Name = ".CYWeb.Session";
            options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);//You can set Time
            options.Cookie.IsEssential = true;
        });

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.AccessDeniedPath = "/AccessDenied";
            options.LoginPath = "/Login";
        });
        #endregion

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

            #region Report
            mc.AddProfile<ReportMappingProfile>();
            #endregion

            #region Supplier
            mc.AddProfile<SupplierMappingProfile>();
            #endregion

            #region Transaction
            mc.AddProfile<TransactionsMappingProfile>();
            #endregion

            #region Money Transfer
            mc.AddProfile<EditMoneyTransferMappingProfile>();
            #endregion
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
        #endregion

        #region Service
        services.AddHttpContextAccessor();
        //services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

        services.AddHttpClient<IHttpClientRequest, HttpClientRequest>("ApiClient");
        //services.AddHttpClient<IHttpClientRequest, HttpClientRequest>("ApiClient").ConfigureHttpClient(client =>
        //{
        //    //s.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
        //    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        //    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        //}).ConfigurePrimaryHttpMessageHandler(() =>
        //{
        //    //var handler = new SocketsHttpHandler();
        //    //handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli;
        //    //return handler;

        //    return new HttpClientHandler
        //    {
        //        AutomaticDecompression = DecompressionMethods.None
        //        //AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
        //    };
        //});
        services.AddSingleton<IDateTimeProvider, DateTimeService>();
		#endregion

		#region Common
		services.AddSingleton<IAppConfig, AppConfig>();
		services.AddSingleton<CYRetailIMS.Application.Common.Interfaces.ILog4NetLogger, CYRetailIMS.Infrastructure.Common.Logging.Log4NetLogger>();
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
        services.AddScoped<IAdjustItemAPI, AdjustItemAPI>();
        services.AddScoped<IAdjustItemTypeAPI, AdjustItemTypeAPI>();
		services.AddScoped<IPurchaseOrderAPI, PurchaseOrderAPI>();
		services.AddScoped<ICurrencyAPI, CurrencyAPI>();
		services.AddScoped<IPaymentTypeAPI, PaymentTypeAPI>();
		services.AddScoped<IPurchaseTypeAPI, PurchaseTypeAPI>();
		services.AddScoped<IShipmentTypeAPI, ShipmentTypeAPI>();
		services.AddScoped<ISupplierAPI, SupplierAPI>();
		services.AddScoped<ISupplierContactTypeAPI, SupplierContactTypeAPI>();
		services.AddScoped<IWarehouseAPI, WarehouseAPI>();
        services.AddScoped<ISupplierTypeAPI, SupplierTypeAPI>();
        services.AddScoped<IChartAPI, ChartAPI>();
        services.AddScoped<IMoneyTransferAPI, MoneyTransferAPI>();
        services.AddScoped<IExcelAPI, ExcelAPI>();
        services.AddScoped<IMoneyTransferSlipAPI, MoneyTransferSlipAPI>();
        services.AddScoped<ISubItemTypeAPI, SubItemTypeAPI>();
        services.AddScoped<ICountStockAPI, CountStockAPI>();
        #endregion

        services.AddControllersWithViews();
        return services;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder webApplicationBuilder, IWebHostEnvironment webHostEnvironment)
    {
        #region Add Log4net
        if (webHostEnvironment.IsProduction())
        {
            webApplicationBuilder.Logging.AddLog4Net("log4net.config");
        }
        else
        {
            webApplicationBuilder.Logging.AddLog4Net($"log4net.{webHostEnvironment.EnvironmentName}.config");
        }
        #endregion
        return webApplicationBuilder;
    }
}

