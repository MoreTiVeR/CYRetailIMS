using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/menu")]
public class MenuController : BaseApiController
{
    public MenuController(ILog4NetLogger log) : base(log)
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
    [HttpGet]
    [Route("v1/getmenubyroleid/{roleid}")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMenuByRoleIDAsync(int roleid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetMenuByRoleIDResponseDTO>> res = await Mediator.Send(new GetMenuByRoleIDQuery { RoleID = roleid });
        Response.Headers.Add("responsecode", res.Status);
        Response.Headers.Add("responsedatasource", res.Soruce);
        Response.Headers.Add("responsemessage", res.Message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetMenuByRoleIDAsync Success");
        return Ok(res.Data);
    }
}
