using System.Collections.Generic;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/itemtype")]
[ApiController]
public class ItemTypeController : BaseApiController
{
    public ItemTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("v1/getitemtypebyid/{itemtypeid}")]
    [ProducesResponseType(typeof(GetItemTypeByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTypeByIDAsync(int itemtypeid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetItemTypeByIDResponseDTO> res = await Mediator.Send(new GetItemTypeByIDQuery { itemtypeid = itemtypeid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTypeByIDAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/getitemtypelist")]
    [ProducesResponseType(typeof(List<GetItemTypeListResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTypeListAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetItemTypeListResponseDTO>> res = await Mediator.Send(new GetItemTypeListQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTypeListAsync Success");
        return Ok(res.data);
    }
}
