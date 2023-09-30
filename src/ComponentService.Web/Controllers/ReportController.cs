using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.Report;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.AreaSale)]
public class ReportController : BaseController
{
    private readonly IReportAPI _reportAPI;
    public ReportController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
		IReportAPI reportAPI) : base(httpClientRequest, mapper, log)
    {
        _reportAPI = reportAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SaleReport()
    {
		BaseResponse<List<SaleReportResponseDTO>> resReport = await _reportAPI.GetSaleReport(new SaleReportQuery
        {
            transaction_startdate = DateTime.Now,
            transaction_enddate = DateTime.Now
        });

        ViewBag.SaleReportList = resReport;
		return View();
    }

    public async Task<IActionResult> SaleSummaryReportAsync()
    {
        BaseResponse<List<SaleSummaryReportResponseDTO>> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReport(new SaleSummaryReportQuery
		{
            transactiondate  = DateTime.Now
        });
		ViewBag.SaleSummaryReportList = resSaleSummaryReport;
		return View();
	}

    public async Task<IActionResult> AuditReportAsync()
	{
		BaseResponse<List<AuditReportResponseDTO>> resAuditReport = await _reportAPI.GetAuditReport(new AuditReportQuery
		{
			transaction_startdate = DateTime.Now,
			transaction_enddate = DateTime.Now
		});

		ViewBag.AuditReportList = resAuditReport;
		return View();
    }
}
