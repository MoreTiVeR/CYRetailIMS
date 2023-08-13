using CYRetailIMS.ComponentService.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Net.Mail;
using System.Net;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Exceptions;
using CYRetailIMS.Application.Common.Models;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class ErrorsController : Controller
{
    [AllowAnonymous]
    public IActionResult CustomError()
    {
        // Gets the status code from the exception or web server.
        var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var statusCode = feature?.Error is HttpException httpEx ? httpEx.StatusCode : (HttpStatusCode)Response.StatusCode;

        // For API errors, responds with just the status code (no page).
        if (HttpContext.Features.Get<IHttpRequestFeature>().RawTarget.StartsWith("/api/", StringComparison.Ordinal))
        {
            return StatusCode((int)statusCode);
        }

        // Creates a view model for a user-friendly error page.
        string text = null;
        switch (statusCode)
        {
            case HttpStatusCode.NotFound: text = $"Page Not Found"; break;
            case HttpStatusCode.InternalServerError: text = "Internal Server Error"; break;
        }

        var errModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = (int)statusCode,
            StatusDescription = text,
            Path = feature != null ? ((Microsoft.AspNetCore.Diagnostics.ExceptionHandlerFeature)feature).Path : null,
            ErrorMsg = feature != null ? $"ClientIP : {feature.Error.Message} | ErrorMsg : {feature.Error.Message}" : null,
            //ErrorStackTrace = feature != null ? feature.Error.StackTrace : "Page Not Found"
        };
        return View("Error", errModel);
    }

    public IActionResult Error()
    {
        return View();
    }

    //[AllowAnonymous]
    //public IActionResult CustomError(ErrorViewModel errModel)
    //{
    //    // Gets the status code from the exception or web server.
    //    IExceptionHandlerFeature feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
    //    HttpStatusCode statusCode = feature?.Error is HttpException httpEx ? httpEx.StatusCode : (HttpStatusCode)Response.StatusCode;

    //    // For API errors, responds with just the status code (no page).
    //    if (HttpContext.Features.Get<IHttpRequestFeature>().RawTarget.StartsWith("/api/", StringComparison.Ordinal))
    //    {
    //        return StatusCode((int)statusCode);
    //    }

    //    // Creates a view model for a user-friendly error page.
    //    switch (statusCode)
    //    {
    //        case HttpStatusCode.NotFound: 
    //            errModel.StatusDescription = $"Page/Data Not Found";
    //            errModel.StatusCode = (int)HttpStatusCode.NotFound;
    //            break;
    //        case HttpStatusCode.InternalServerError: 
    //            errModel.StatusDescription = "Internal Server Error";
    //            errModel.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            break;
    //        case HttpStatusCode.Unauthorized:
    //            errModel.StatusDescription = "Unauthorized";
    //            errModel.StatusCode = (int)HttpStatusCode.Unauthorized;
    //            break;
    //        default: 
    //            errModel.StatusDescription = "Internal Server Error";
    //            errModel.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            break;
    //    }

    //    errModel.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    //    errModel.Path = ((Microsoft.AspNetCore.Diagnostics.ExceptionHandlerFeature)feature)?.Path;
    //    //ErrorViewModel errModel = new ErrorViewModel
    //    //{
    //    //    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
    //    //    StatusCode = (int)statusCode,
    //    //    StatusDescription = text,
    //    //    Path = ((Microsoft.AspNetCore.Diagnostics.ExceptionHandlerFeature)feature)?.Path,
    //    //    ErrorMsg = feature?.Error?.Message,
    //    //    RequestController = exceptionSettings.ExceptionController,
    //    //    RequestAction = exceptionSettings.ExceptionAction
    //    //};
    //    //LogHelper.log.Error($"CustomError : {JsonConvert.SerializeObject(errModel)}");

    //    return View("Error", errModel);
    //    //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
}
