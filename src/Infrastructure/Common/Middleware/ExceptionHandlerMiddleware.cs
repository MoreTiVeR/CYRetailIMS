using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CYRetailIMS.Infrastructure.Common.Middleware;
public class ExceptionHandlerMiddleware
{
    private readonly ExceptionSettings _exceptionSettings;
    private readonly RequestDelegate _next;
    public ExceptionHandlerMiddleware(RequestDelegate next, IOptionsMonitor<ExceptionSettings> exceptionSettings)
    {
        _next = next;
        _exceptionSettings = exceptionSettings.CurrentValue;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        // Here you can customize how you want to handle the exception
        // For example, logging the error, setting response status code, etc.
        // You can also create a custom error response in JSON or HTML format.
        string route = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";

        // Set the response status code to 500 (Internal Server Error)
        ErrorData errorData = new ErrorData
        {
            type = StatusCodes.Status500InternalServerError.ToString(),
            status = StatusCodes.Status500InternalServerError.ToString(),
            message = ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) ? ex.InnerException.Message : ex.Message,
            path = route,
            stracktrace = _exceptionSettings.IsDeveloperMode 
            ? ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) 
            ? ex.InnerException.StackTrace : ex.StackTrace : ex.StackTrace
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.Headers.Add("x-status", StatusCodes.Status500InternalServerError.ToString());
        httpContext.Response.Headers.Add("x-source", ex.TargetSite.Module.Assembly.FullName.ToString().Split(",").FirstOrDefault());
        httpContext.Response.Headers.Add("x-message", errorData.message);
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return httpContext.Response.WriteAsync(errorData.ToJson());
    }

}
