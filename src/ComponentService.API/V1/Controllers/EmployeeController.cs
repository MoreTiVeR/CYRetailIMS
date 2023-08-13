using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/employee")]
public class EmployeeController : BaseApiController
{
    public EmployeeController(ILog4NetLogger log) : base(log)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Endpoint: 
    ///     
    ///     GET: {host}/employee/v1/create
    ///     Sample Request:
    /// 
    ///     {
    ///         "empcode": "001"
    ///         "firstname": "สมชาย"
    ///         "lastname": "จิตรดร"
    ///         "createdby": "CF305568"
    ///     }
    ///     
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEmployeeAsync(CreateEmployeeCommand request)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(request);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateEmployeeAsync Success");

        return Ok(res.data);
    }
}
