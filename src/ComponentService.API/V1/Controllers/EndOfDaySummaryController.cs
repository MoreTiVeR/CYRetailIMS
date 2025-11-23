using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.CreateEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.UpdateEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryList.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceipt.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/eodsummary")]
[ApiController]
public class EndOfDaySummaryController : BaseApiController
{
    public EndOfDaySummaryController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/inquiry")]
    [ProducesResponseType(typeof(GetEndOfDaySummaryByCriteriaResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InquiryEODSummaryAsync(GetEndOfDaySummaryByCriteriaQuery eodSummaryByCriteriaQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetEndOfDaySummaryByCriteriaResponseDTO> res = await Mediator.Send(eodSummaryByCriteriaQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]InquiryEODSummaryAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEodSummaryAsync(CreateEndOfDaySummaryCommand createEndOfDaySummaryCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createEndOfDaySummaryCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateEodSummaryAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/update")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEodSummaryAsync(UpdateEndOfDaySummaryCommand updateEndOfDaySummaryCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(updateEndOfDaySummaryCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateEodSummaryAsync Success");
        return Ok(res.data);
    }
}
