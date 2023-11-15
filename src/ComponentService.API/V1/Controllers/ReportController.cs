using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using Microsoft.AspNetCore.Mvc;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByTransID.v1;
using CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;
[Route("api/v{version:apiVersion}/report")]
[ApiController]
public class ReportController : BaseApiController
{
    public ReportController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/createaudittransaction")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAuditTransactionReportAsync(CreateAuditReportCommand createAuditReportCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createAuditReportCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateAuditTransactionReportAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/salereport")]
    [ProducesResponseType(typeof(List<SaleReportResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaleReportAsync(SaleReportQuery saleReportQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<SaleReportResponseDTO>> res = await Mediator.Send(saleReportQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]SaleReportAsync Success");
        return Ok(res.data);
    }

    /// <summary>
    /// รายงานสรุปยอดขายสิ้นวัน แสดงยอดรวมทั้งหมด แต่ละสาขามี 1 รายการ/วัน เท่านั้น
    /// </summary>
    /// <param name="saleSummaryReportQuery"></param>
    /// <returns></returns>
	[HttpPost]
    [Route("v1/salesummaryreport")]
    [ProducesResponseType(typeof(List<SaleSummaryReportResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaleSummaryReportAsync(SaleSummaryReportQuery saleSummaryReportQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<SaleSummaryReportResponseDTO>> res = await Mediator.Send(saleSummaryReportQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]SaleSummaryReportAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/salesummaryreportbytransid/{transactionid:int}")]
    [ProducesResponseType(typeof(List<SaleSummaryReportResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaleSummaryReportByTransIDAsync(int transactionid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<SaleSummaryReportResponseDTO> res = await Mediator.Send(new SaleSummaryReportByTransIDQuery { transactionid = transactionid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]SaleSummaryReportByTransIDAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/salesummaryreportbybranch")]
    [ProducesResponseType(typeof(List<SaleSummaryReportResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaleSummaryReportByBranchsync(SaleSummaryReportByBranchQuery reportByBranchIDQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<SaleSummaryReportResponseDTO> res = await Mediator.Send(reportByBranchIDQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]SaleSummaryReportByBranchsync Success");
        return Ok(res.data);
    }


    [HttpPost]
    [Route("v1/auditreport")]
    [ProducesResponseType(typeof(List<AuditReportResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AuditReportAsync(AuditReportQuery summaryReportQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<AuditReportResponseDTO>> res = await Mediator.Send(summaryReportQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]AuditReportAsync Success");
        return Ok(res.data);
    }


    /// <summary>
    /// ดึงข้อมูลรายงาน การปรับราคาสินค้า
    /// </summary>
    /// <param name="itemTransactionLogQuery"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/itemtransactionlog")]
    [ProducesResponseType(typeof(List<ItemTransactionLogReportResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ItemTransactionLogReportAsync(ItemTransactionLogReportQuery itemTransactionLogQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<ItemTransactionLogReportResponseDTO>> res = await Mediator.Send(itemTransactionLogQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]ItemTransactionLogReportAsync Success");
        return Ok(res.data);
    }

}
