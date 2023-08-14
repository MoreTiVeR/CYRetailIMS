using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureByID.v1;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/unitofmeasure")]
[ApiController]
public class UnitOfMeasureController : BaseApiController
{
    public UnitOfMeasureController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("v1/getunitofmeasure")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUnitOfMeasureListAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetUnitOfMeasureListResponseDTO>> res = await Mediator.Send(new GetUnitOfMeasureListQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetUnitOfMeasureListAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/getunitofmeasurebyid/{unitofmeasureid}")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUnitOfMeasureByIDAsync(int unitofmeasureid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetUnitOfMeasureByIDResponseDTO> res = await Mediator.Send(new GetUnitOfMeasureByIDQuery { unitofmeasureid = unitofmeasureid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetUnitOfMeasureByIDAsync Success");
        return Ok(res.data);
    }
}