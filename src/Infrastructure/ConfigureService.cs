using CYRetailIMS.Application.Common.Confiuration;
using CYRetailIMS.Application.Common.Cryptography;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Domain.Infrastructure.Database;
using CYRetailIMS.Domain.Infrastructure.Repositories;
using CYRetailIMS.Infrastructure.Common.Configuration;
using CYRetailIMS.Infrastructure.Common.Cryptography;
using CYRetailIMS.Infrastructure.Common.Logging;
using CYRetailIMS.Infrastructure.Common.Service;
using CYRetailIMS.Infrastructure.Database;
using CYRetailIMS.Infrastructure.ExternalService.ItemAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemBrand;
using CYRetailIMS.Infrastructure.ExternalService.ItemTypeAPI;
using CYRetailIMS.Infrastructure.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CYRetailIMS.Infrastructure;
public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CYDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            builder => builder.MigrationsAssembly(typeof(CYDBContext).Assembly.FullName)),
            ServiceLifetime.Scoped);

        #region Common
        services.AddTransient<IAppConfig, AppConfig>();
        services.AddTransient<IEncryptionString, EncryptionString>();
        services.AddTransient<ILog4NetLogger, Log4NetLogger>();
        #endregion

        #region Repositories & Database
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        #endregion

        #region Service
        services.AddTransient<IDateTimeProvider, DateTimeService>();
        #endregion

        return services;
    }
}