
using CYRetailIMS.Application.Common.Confiuration;
using Microsoft.Extensions.Configuration;

namespace CYRetailIMS.Infrastructure.Common.Configuration;
public class AppConfig : IAppConfig
{
    private IConfigurationRoot Configuration { get; set; }

    public AppConfig()
    {
        if (Configuration == null)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env}.json", true, true);
            Configuration = builder.Build();
        }
    }

    public string GetConnectionStringDefault() => Configuration.GetSection("ConnectionStrings:DefaultConnection").Get<string>();

    public string GetUserSecretKey() => Configuration.GetSection("AppSettings:SECRET_KEY").Get<string>();

    public string GetImportItemFilePath() => Configuration.GetSection("AppSettings:IMPORT_FILE_PATH").Get<string>();
}
