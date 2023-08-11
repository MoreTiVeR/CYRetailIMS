
using System.Configuration;
using CYRetailIMS.Application;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Infrastructure;
using CYRetailIMS.Infrastructure.Common.Middleware;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;


namespace CYRetailIMS.ComponentService.API;

public class Program
{
    private static IConfiguration _configuration;
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var env = builder.Environment.EnvironmentName;
        _configuration = builder.Configuration;

        #region Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddApplicationServices();
        builder.Services.AddComponentServices(_configuration, builder.Environment.EnvironmentName);
        builder.Services.AddInfrastructureServices(_configuration);
        builder.Services.Configure<ExceptionSettings>(_configuration.GetSection("ExceptionSettings"));
        #endregion

        #region Add Swagger

        #endregion
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        ILoggerFactory loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseStaticFiles();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsProduction())
        {
            loggerFactory.AddLog4Net("log4net.config");
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
                //c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "V1");
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
        else
        {
            loggerFactory.AddLog4Net($"log4net.{app.Environment.EnvironmentName}.config");

            #region specifying the Swagger JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
                //c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "V1");
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
            #endregion
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        //ExceptionSettings exceptionSettings = _configuration.GetSection("ExceptionSettings").Get<ExceptionSettings>();
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.MapControllers();

        app.Run();
    }

    private static bool IsValidIPAddress(string ipAddress) => System.Text.RegularExpressions.Regex.IsMatch(ipAddress, @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");

}
