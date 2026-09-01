using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ApproveStatusService.Queries.GetApproveStatus.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/department")]
[ApiController]
public class DepartmentController : BaseApiController
{
    public DepartmentController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("v1/getdepartments")]
    [ProducesResponseType(typeof(List<GetDepartmentsResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDepartmentsAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetDepartmentsResponseDTO>> res = await Mediator.Send(new GetDepartmentQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetDepartmentsAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/getdepartmentbyid/{departmentid:int}")]
    [ProducesResponseType(typeof(GetDepartmentByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDepartmentByIDAsync(int departmentid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetDepartmentByIDResponseDTO> res = await Mediator.Send(new GetDepartmentByIDQuery { departmentid = departmentid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetDepartmentByIDAsync Success");
        return Ok(res.data);
    }
}
