using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/role")]
[ApiController]
public class RolesController : BaseApiController
{
    public RolesController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("v1/getroles")]
    [ProducesResponseType(typeof(List<GetRolesResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolesAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetRolesResponseDTO>> res = await Mediator.Send(new GetRolesQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetRolesAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/getrolebyid/{roleid:int}")]
    [ProducesResponseType(typeof(GetRoleByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoleByIDAsync(int roleid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetRoleByIDResponseDTO> res = await Mediator.Send(new GetRoleByIDQuery { roleid = roleid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetRoleByIDAsync Success");
        return Ok(res.data);
    }
}
