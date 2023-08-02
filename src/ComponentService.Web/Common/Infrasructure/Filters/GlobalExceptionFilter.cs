using CYRetailIMS.Application.Common.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CYRetailIMS.ComponentService.Web.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CYRetailIMS.ComponentService.Web.Common.Infrasructure.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly ExceptionSettings _exceptionSettings;
    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IOptionsMonitor<ExceptionSettings> exceptionSettings)
    {
        _logger = logger;
        _exceptionSettings = exceptionSettings.CurrentValue;
    }

    public void OnException(ExceptionContext context)
    {
        // Log the exception
        _logger.LogError(context.Exception, "An unhandled exception occurred.");
        var action = context.RouteData.Values["action"].ToString();
        var controller = context.RouteData.Values["controller"].ToString();
        // Set the result to a custom error page or JSON response
        // In this example, I'm returning a JSON response with the exception message.
        //context.Result = new JsonResult(new { error = "An unexpected error occurred." })
        //{
        //    StatusCode = 500
        //};

        // Mark the exception as handled
        context.ExceptionHandled = true;

        //Redirect to action
        //context.Result = RedirectToAction("Error", "InternalError");

        //context.Result = new ViewResult()
        //{
        //    ViewName = "Error"
        //};

        ErrorViewModel errorData = new ErrorViewModel
        {
            StatusCode = (int)StatusCodes.Status500InternalServerError,
            StatusDescription = "Internal Server Error",
            IsDeveloperMode = _exceptionSettings.IsDeveloperMode,
            ErrorMsg = context.Exception.InnerException != null ? context.Exception.InnerException.Message : context.Exception.Message,
            RequestAction = context.RouteData.Values["action"].ToString(),
            RequestController = context.RouteData.Values["controller"].ToString()
        };
        context.Result = new RedirectToActionResult("Index", "Errors", errorData);
    }
}
