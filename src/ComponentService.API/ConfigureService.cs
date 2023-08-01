using System.ComponentModel;
using System.Reflection;
using System.Text;
using CYRetailIMS.Application.Common.Filter;
using CYRetailIMS.Application.Common.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CYRetailIMS.ComponentService.API;

public static class ConfigureService
{
    public static IServiceCollection AddComponentServices(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        services.AddHttpContextAccessor();
        services.AddFluentValidationAutoValidation(config =>
        {
            //Set true for disable DataAnnotation validation then use Fluent Validation
            config.DisableDataAnnotationsValidation = true;
        });
        services.Configure<ApiBehaviorOptions>(options =>
        {
            //Gets or sets a value that determines if the filter that returns an Microsoft.AspNetCore.Mvc.BadRequestObjectResult
            options.SuppressModelStateInvalidFilter = true;
        });

        #region Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            //c.SwaggerDoc("v1", new OpenApiInfo
            //{
            //    Version = "v1",
            //    Title = "CY Retail Inventory Management System API",
            //    Description = $"A CY Retail Inventory Management System API <b>(.NET7)</b> <b>Ver.{Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}</b> <b>({envName})</b>",
            //    Contact = new OpenApiContact
            //    {
            //        Name = "AppBoxs Team",
            //        Email = "email address"
            //    },
            //    License = new OpenApiLicense
            //    {
            //        Name = Assembly.GetExecutingAssembly().GetName().Name
            //    }
            //});
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme {Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, new string[] {}
                }
            });
            //c.OperationFilter<HeaderFilter>();
            c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}/{apiDesc.HttpMethod}");
            c.CustomSchemaIds(a => a.FullName);
        });
        #endregion

        #region Api Versioning Control
        services.AddApiVersioning(options => options.ReportApiVersions = true);
        //services.AddApiVersioning(o => {
        //    o.ReportApiVersions = true;
        //    o.AssumeDefaultVersionWhenUnspecified = true;
        //    o.DefaultApiVersion = new ApiVersion(1, 0);

        //    //o.Conventions.Controller<HomeV1Controller>().HasApiVersion(new ApiVersion(1, 0));
        //    //o.Conventions.Controller<HomeV2Controller>().HasApiVersion(new ApiVersion(2, 0));
        //});

        services.AddVersionedApiExplorer(
                 options =>
                 {
                     //The format of the version added to the route URL
                     options.GroupNameFormat = "'v'VVV";
                     //Tells swagger to replace the version in the controller route
                     options.SubstituteApiVersionInUrl = true;
                 });

        services.AddSwaggerGen(
            options =>
            {
                // Resolve the temprary IApiVersionDescriptionProvider service
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                // Add a swagger document for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Version = description.ApiVersion.ToString(),
                        Title = $"CY Retail Inventory Management System API",
                        Description = "Ying Charoen Retail Inventory Management System API",
                        TermsOfService = new Uri("https://www.case-yingcharoen.com/terms"),
                        Contact = new OpenApiContact() { Name = "APPBOXS", Email = "eng.nattapong@gmail.com", Url = new Uri("https://www.case-yingcharoen.com") },
                        License = new OpenApiLicense() { Name = "APPBOXS", Url = new Uri("https://www.case-yingcharoen.com") }
                    });
                }

                // Add a custom filter for settint the default values
                options.OperationFilter<SwaggerDefaultValues>();

                // Tells swagger to pick up the output XML document file
                //options.IncludeXmlComments(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{this.GetType().Assembly.GetName().Name}.xml"));

            });
        #endregion

        return services;
    }
}
