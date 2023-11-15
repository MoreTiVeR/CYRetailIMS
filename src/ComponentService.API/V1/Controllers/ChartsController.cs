using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v2;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryByYear.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/chart")]
[ApiController]
public class ChartsController : BaseApiController
{
    public ChartsController(ILog4NetLogger log) : base(log)
    {
    }


    [HttpPost]
    [Route("v1/montlysale")]
    [ProducesResponseType(typeof(List<GetMontlySaleSummaryBarchartResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMontlySameSummaryAsync(GetMontlySaleSummaryBarchartQuery getMontlySaleSummary)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>> res = await Mediator.Send(getMontlySaleSummary);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetMontlySameSummaryAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v2/montlysale")]
    [ProducesResponseType(typeof(List<GetMontlySaleSummaryBarchartResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMontlySameSummaryV2Async(GetMontlySaleSummaryBarchartV2Query getMontlySaleSummary)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>> res = await Mediator.Send(getMontlySaleSummary);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetMontlySameSummaryV2Async Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/yearsale")]
    [ProducesResponseType(typeof(List<GetMontlySaleSummaryByYearResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetYearlySameSummaryAsync(GetMontlySaleSummaryByYearQuery summaryByYearQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetMontlySaleSummaryByYearResponseDTO>> res = await Mediator.Send(summaryByYearQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetYearlySameSummaryAsync Success");
        return Ok(res.data);
    }
}
