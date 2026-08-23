using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ExcelService.Queries.GenerateStockTransferExcelReport.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/excel")]
[ApiController]
public class ExcelController : BaseApiController
{
    public ExcelController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/generate")]
    [ProducesResponseType(typeof(GenerateStockTransferExcelReportResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateInventoryTransferExcelAsync(GenerateStockTransferExcelReportQuery excelReportQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GenerateStockTransferExcelReportResponseDTO> res = await Mediator.Send(excelReportQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GenerateInventoryTransferExcelAsync Success");
        return Ok(res.data);
    }
}
