using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.Report;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockByBrachReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;
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

    /// <summary>
    /// รายงานแสดงสินค้าขั้นต่ำ
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> ItemQtyReport()
    {
		BaseResponse<List<GetBranchResponseDTO>> resBranchList = await _branchAPI.GetBranchListAsync();
        ViewBag.BranchList = resBranchList;
		return View();
    }

	/// <summary>
	/// รายงานปรับราคาสินค้าหน้าร้าน
	/// </summary>
	/// <returns></returns>
	public async Task<IActionResult> ItemTransactionReport()
    {
		BaseResponse<List<GetBranchResponseDTO>> resBranchList = await _branchAPI.GetBranchListAsync();
		ViewBag.BranchList = resBranchList;
		return View();
    }

	/// <summary>
	/// ดึงข้อมูล รายงานแสดงสินค้าขั้นต่ำ
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> GetAvailableItemQtyReport()
	{
		List<AvailableStockReportResponseDTO> resItemQtyList = null;
		try
		{
			if (base.UserProfile.roleid == (int)EnumModel.UserRole.Admin)
			{
				//สินค้าคลังใหญ่
				BaseResponse<List<AvailableStockReportResponseDTO>> resItem = await _reportAPI.GetAvailableItemStockReportAsync(new AvailableStockReportQuery());
				if (!resItem.result)
				{
					return Json(new { result = false, data = new List<AvailableStockReportResponseDTO>(), message = "ไม่มีสินค้าหน้าร้าน" });
				}
				resItemQtyList = resItem.data;
			}
			else
			{
				//สินค้าคลังสาขา
				BaseResponse<List<AvailableStockReportResponseDTO>> resItemInBranch = await _reportAPI.GetAvailableItemStockByBranchReportAsync(new AvailableStockByBrachReportQuery
                {
                    branchid = base.UserProfile.access_branch.FirstOrDefault().branchid
				});
				if (!resItemInBranch.result)
				{
					return Json(new { result = false, data = new List<AvailableStockReportResponseDTO>(), message = "ไม่มีสินค้าหน้าร้าน" });
				}
				resItemQtyList = resItemInBranch.data;
			}
			return Json(new { data = resItemQtyList });
		}
		catch(Exception ex)
		{
			return Json(new { result = false, data = new List<AvailableStockReportResponseDTO>(), message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}"});
		}
	}

	/// <summary>
	/// ค้นหาข้อมูล รายงานสินค้าขั้นต่ำ แบ่งตามสาขา
	/// </summary>
	/// <param name="branchid"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<JsonResult> SearchAvailableItemQtyByBranch(int branchid)
	{
		List<AvailableStockReportResponseDTO> resItemQtyList = null;
		try
		{
			if (branchid == 1)
			{
				//สินค้าคลังใหญ่
				BaseResponse<List<AvailableStockReportResponseDTO>> resAvailableItem = await _reportAPI.GetAvailableItemStockReportAsync(new AvailableStockReportQuery());
				if (!resAvailableItem.result)
				{
					return Json(new { result = false, data = new List<AvailableStockReportResponseDTO>(), message = "ไม่พบสินค้าที่อยู่ในเกณฑ์ขั้นต่ำ" });
				}
				resItemQtyList = resAvailableItem.data;
			}
			else
			{
				//สินค้าคลังสาขา
				BaseResponse<List<AvailableStockReportResponseDTO>> resAvailableItemIBranch = await _reportAPI.GetAvailableItemStockByBranchReportAsync(new AvailableStockByBrachReportQuery { branchid = branchid });
				if (!resAvailableItemIBranch.result)
				{
					return Json(new { result = false, data = new List<AvailableStockReportResponseDTO>(), message = "ไม่พบสินค้าที่อยู่ในเกณฑ์ขั้นต่ำ" });
				}
                resItemQtyList = resAvailableItemIBranch.data;
			}
			return Json(new { result = true, data = resItemQtyList, message = "สำเร็จ" });
		}
		catch (Exception ex)
		{
			return Json(new { result = false, data = new List<AvailableStockReportResponseDTO>(), message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}" });
		}
	}

	/// <summary>
	/// รายงานปรับราคาสินค้าหน้าร้าน
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public async Task<IActionResult> GetItemPriceTransactionReport()
	{
		try
		{
            int branchId = base.UserProfile.roleid == (int)EnumModel.UserRole.Admin ? 1 : base.UserProfile.access_branch.FirstOrDefault().branchid;
			BaseResponse<List<ItemTransactionLogReportResponseDTO>> resItem = await _reportAPI.GetItemTransactionLogReportAsync(new ItemTransactionLogReportQuery { branchid = branchId });
			if (!resItem.result)
			{
				return Json(new { result = false, data = new List<ItemTransactionLogReportResponseDTO>(), message = "ไม่พบข้อมูลการเปลี่ยนแปลงราคาสินค้า" });
			}
			return Json(new { data = resItem.data });
		}
		catch (Exception ex)
		{
			return Json(new { result = false, data = new List<ItemTransactionLogReportResponseDTO>(), message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}" });
		}
	}

	/// <summary>
	/// ค้นหาข้อมูล รายงานปรับราคาสินค้าหน้าร้าน ตามสาขา
	/// </summary>
	/// <param name="branchid"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<JsonResult> SearchItemPriceTransactionReportByBranch(int branchid)
	{
		try
		{
			BaseResponse<List<ItemTransactionLogReportResponseDTO>> resItem = await _reportAPI.GetItemTransactionLogReportAsync(new ItemTransactionLogReportQuery { branchid = branchid });
			if (!resItem.result)
			{
				return Json(new { result = false, data = new List<ItemTransactionLogReportResponseDTO>(), message = "ไม่พบข้อมูลการเปลี่ยนแปลงราคาสินค้า" });
			}
			return Json(new { result = true, data = resItem.data, message = "สำเร็จ" });
		}
		catch (Exception ex)
		{

			return Json(new { result = false, data = new List<ItemTransactionLogReportResponseDTO>(), message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}" });
		}
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
            createdby = base.UserProfile.username,
            transactiondatetime = reportData.TransactionDate.ToDateTime(),
            //createddate = $"{reportData.TransactionDate} {DateTime.Now:HH}:{DateTime.Now:mm}:{DateTime.Now:ss}".ToDateTime(),
            createddate = DateTime.Now
        };
    }

}
