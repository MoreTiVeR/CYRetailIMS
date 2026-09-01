using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByID.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByItemIDList.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/subitemtype")]
[ApiController]
public class SubItemTypeController : BaseApiController
{
    public SubItemTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSubItemTypeAsync(CreateSubItemTypeCommand subItemTypeCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(subItemTypeCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateSubItemTypeAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/subitemtypebyid")]
    [ProducesResponseType(typeof(GetSubItemTypeResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSubItemTypeByIDAsync(GetSubItemTypeByIDQuery subItemTypeByIDQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetSubItemTypeResponseDTO> res = await Mediator.Send(subItemTypeByIDQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetSubItemTypeByIDAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/subitemtypelist")]
    [ProducesResponseType(typeof(List<GetSubItemTypeResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllSubItemTypeAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetSubItemTypeResponseDTO>> res = await Mediator.Send(new GetSubItemTypeListQuery { });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetAllSubItemTypeAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/subitemtypebyitemids")]
    [ProducesResponseType(typeof(List<GetSubItemTypeByItemIDListResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSubItemTypeByItemIDListAsync(GetSubItemTypeByItemIDListQuery itemidlistQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetSubItemTypeByItemIDListResponseDTO>> res = await Mediator.Send(itemidlistQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetSubItemTypeByItemIDListAsync Success");
        return Ok(res.data);
    }
}
