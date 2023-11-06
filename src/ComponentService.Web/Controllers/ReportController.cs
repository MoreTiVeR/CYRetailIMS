using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.Report;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;
using CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.AreaSale)]
public class ReportController : BaseController
{
    private readonly IReportAPI _reportAPI;
    private readonly IBranchAPI _branchAPI;
    public ReportController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
		IReportAPI reportAPI, IBranchAPI branchAPI) : base(httpClientRequest, mapper, log)
    {
        _reportAPI = reportAPI;
        _branchAPI = branchAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SaleReport()
    {
		BaseResponse<List<SaleReportResponseDTO>> resReport = await _reportAPI.GetSaleReportAsync(new SaleReportQuery
        {
            transaction_startdate = DateTime.Now,
            transaction_enddate = DateTime.Now
        });

        ViewBag.SaleReportList = resReport;
		return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetSaleReport()
    {
        try
        {
            BaseResponse<List<SaleReportResponseDTO>> resReport = await _reportAPI.GetSaleReportAsync(new SaleReportQuery
            {
                transaction_startdate = DateTime.Now,
                transaction_enddate = DateTime.Now
            });
            if (!resReport.result)
            {
                throw new Exception(resReport.error.error.message);
            }
            return Json(new { data = resReport.data });
        }
        catch
        {
            return Json(new { data = new List<SaleReportResponseDTO>() });
        }
    }

    /// <summary>
    /// สรุปยอดรวมประจำวัน ของแต่ละสาขา 1 สาขามี 1 รายการ /1วัน
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> SaleSummaryReportAsync()
    {
        //BaseResponse<List<SaleSummaryReportResponseDTO>> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportAsync(new SaleSummaryReportQuery
		//{
        //    transactiondate  = DateTime.Now
        //});
		//ViewBag.SaleSummaryReportList = resSaleSummaryReport;
		return View();
	}

    [HttpGet]
    public async Task<IActionResult> GetSaleSummaryReport()
    {
        try
        {
            BaseResponse<List<SaleSummaryReportResponseDTO>> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportAsync(new SaleSummaryReportQuery
            {
                transactiondate = DateTime.Now
            });
            if (!resSaleSummaryReport.result)
            {
                throw new Exception(resSaleSummaryReport.error.error.message);
            }
            return Json(new { data = resSaleSummaryReport.data });
        }
        catch
        {
            return Json(new { data = new List<SaleSummaryReportResponseDTO>() });
        }
    }

    /// <summary>
    /// สรุปยอดรวมประจำวัน ของทุกสาขา 1รายการ/1วัน 
    /// รายงานตั้งแต่วันที่ 1 ของเดือน ถึง end of month
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> AuditReportAsync()
	{
        //BaseResponse<List<AuditReportResponseDTO>> resAuditReport = await _reportAPI.GetAuditReportAsync(new AuditReportQuery
        //{
        //    transaction_startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
        //    transaction_enddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
        //});

		//ViewBag.AuditReportList = resAuditReport;
		return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAuditReport()
    {
        try
        {
            BaseResponse<List<AuditReportResponseDTO>> resAuditReport = await _reportAPI.GetAuditReportAsync(new AuditReportQuery
            {
                transaction_startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                transaction_enddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            });
            if (!resAuditReport.result)
            {
                throw new Exception(resAuditReport.error.error.message);
            }
            return Json(new { data = resAuditReport.data });
        }
        catch
        {
            return Json(new { data = new List<AuditReportResponseDTO>() });
        }
    }

    [Obsolete("*** Move to AuditSaleSummaryReportByBranch(int branchid)")]
    public async Task<IActionResult> AuditSaleSummaryReportByTransaction(int transactionid)
    {
        BaseResponse<SaleSummaryReportResponseDTO> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportByTransIDAsync(transactionid);
        AuditSaleSummaryReportViewModel auditReportViewData = _mapper.Map<AuditSaleSummaryReportViewModel>(resSaleSummaryReport.data);
        return View(auditReportViewData);
    }

    public async Task<IActionResult> AuditSaleSummaryReportByBranch(int branchid)
    {
        BaseResponse<SaleSummaryReportResponseDTO> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportByBranchAsync(new SaleSummaryReportByBranchQuery
        {
            branchid = branchid,
            transactiondate = DateTime.Now
        });
        AuditSaleSummaryReportViewModel auditReportViewData = _mapper.Map<AuditSaleSummaryReportViewModel>(resSaleSummaryReport.data);
        return View(auditReportViewData);
    }

    [HttpPost]
    public async Task<IActionResult> SaveAuditSaleSummaryReport([FromBody] AuditSaleSummaryReportViewModel reportData)
    {
        try
        {
            CreateAuditReportCommand auditReportCommand = MappingCreateAuditReportCommand(reportData);
            BaseResponse<CommandResponse> res = await _reportAPI.CreateAuditTransactionReportAsync(auditReportCommand);
            return Json(new { result = res.result, message = res.result ? "บันทึกข้อมูลสำเร็จ" : $"{res.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    private CreateAuditReportCommand MappingCreateAuditReportCommand(AuditSaleSummaryReportViewModel reportData)
    {
        return new CreateAuditReportCommand
        {
            branchid = reportData.BranchID,
            description = reportData.AuditDescription,
            totalamountaudit = reportData.TotalAuditAmount.Value,
            createdby = base.UserProfile.rolename,
            transactiondatetime = reportData.TransactionDate.ToDateTime(),
            //createddate = $"{reportData.TransactionDate} {DateTime.Now:HH}:{DateTime.Now:mm}:{DateTime.Now:ss}".ToDateTime(),
            createddate = DateTime.Now
        };
    }


    #region Http Method

    #endregion
}
