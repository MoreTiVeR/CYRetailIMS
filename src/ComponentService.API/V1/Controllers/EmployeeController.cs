using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.DeleteEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployeeByID.v1;
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

    [HttpPost]
    [Route("v1/update")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEmployeeAsync(UpdateEmployeeCommand request)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(request);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateEmployeeAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/delete")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployeeAsync(DeleteEmployeeCommand deleteEmployeeCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(deleteEmployeeCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]DeleteEmployeeAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/getemployees")]
    [ProducesResponseType(typeof(List<GetEmployeeResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeesAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetEmployeeResponseDTO>> res = await Mediator.Send(new GetEmployeeQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetEmployeesAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/getemployeebyid/{empid:int}")]
    [ProducesResponseType(typeof(GetEmployeeResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeeByIDAsync(int empid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetEmployeeResponseDTO> res = await Mediator.Send(new GetEmployeeByIDQuery { empid = empid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetEmployeeByIDAsync Success");
        return Ok(res.data);
    }
}
