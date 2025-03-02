using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ReportAPI;
using CYRetailIMS.Application.ExternalService.SubItemTypeAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockByBrachReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.CountStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
using CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.AreaSale)]
public class ReportController : BaseController
{
    private readonly IReportAPI _reportAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly ISubItemTypeAPI _subItemTypeAPI;
    public ReportController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IReportAPI reportAPI, IBranchAPI branchAPI,
        ISubItemTypeAPI subItemTypeAPI) : base(httpClientRequest, mapper, log)
    {
        _reportAPI = reportAPI;
        _branchAPI = branchAPI;
        _subItemTypeAPI = subItemTypeAPI;
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

    [HttpPost]
    public async Task<IActionResult> SearchSaleReport([FromBody] SearchSaleReportViewModel searchObj)
    {
        try
        {
            DateTime sDate = searchObj.startdate.DatetimePickerToDate();
            DateTime eDate = searchObj.enddate.DatetimePickerToDate();
            //StartDate > EndDate
            if (DateTime.Compare(sDate, eDate) == 1)
            {
                throw new Exception("รูปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }

            BaseResponse<List<SaleReportResponseDTO>> resReport = await _reportAPI.GetSaleReportAsync(new SaleReportQuery
            {
                transaction_startdate = sDate,
                transaction_enddate = eDate
            });
            if (!resReport.result)
            {
                return Json(new { result = false, message = resReport.error.error.message, data = new List<SaleReportResponseDTO>() });
            }

            //resReport.data = resReport.data.Where(w => w.createddate.Date >= sDate.Date && w.createddate.Date <= eDate.Date).ToList();
            //if (resReport.data.Count == 0)
            //{
            //	return Json(new { result = false, message = "ไม่พบข้อมูล", data = new List<SaleReportResponseDTO>() });
            //}

            return Json(new { result = true, message = "สำเร็จ", data = resReport.data });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<SaleReportResponseDTO>() });
        }
    }

    /// <summary>
    /// สรุปยอดรวมประจำวัน ของแต่ละสาขา 1 สาขามี 1 รายการ /1วัน
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> SaleSummaryReport()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetSaleSummaryReport()
    {
        try
        {
            int dayInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            BaseResponse<List<SaleSummaryReportResponseDTO>> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportAsync(new SaleSummaryReportQuery
            {
                starttransactiondate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                endtransactiondate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayInMonth)
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

    [HttpPost]
    public async Task<IActionResult> SearchSaleSummaryReport([FromBody] SearchSaleSummaryReportViewModel searchObj)
    {
        try
        {
            DateTime sDate = searchObj.startdate.DatetimePickerToDate();
            DateTime eDate = searchObj.enddate.DatetimePickerToDate();
            //StartDate > EndDate
            if (DateTime.Compare(sDate, eDate) == 1)
            {
                throw new Exception("รูปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }

            BaseResponse<List<SaleSummaryReportResponseDTO>> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportAsync(new SaleSummaryReportQuery
            {
                starttransactiondate = sDate,
                endtransactiondate = eDate,
                branchid = searchObj.branchid
            });
            if (!resSaleSummaryReport.result)
            {
                return Json(new { result = false, message = resSaleSummaryReport.error.error.message, data = new List<SaleSummaryReportResponseDTO>() });
            }
            return Json(new { result = true, message = "สำเร็จ", data = resSaleSummaryReport.data });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<SaleSummaryReportResponseDTO>() });
        }
    }

    /// <summary>
    /// สรุปยอดรวมประจำวัน ของทุกสาขา 1รายการ/1วัน 
    /// รายงานตั้งแต่วันที่ 1 ของเดือน ถึง end of month
    /// </summary>
    /// <returns></returns>
    public IActionResult AuditReport()
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
        catch (Exception ex)
        {
            return Json(new { result = false, data = new List<AvailableStockReportResponseDTO>(), message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}" });
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

    [HttpPost]
    public async Task<IActionResult> SearchAuditReport([FromBody] SearchAuditReportViewModel searchObj)
    {
        try
        {
            DateTime sDate = searchObj.startdate.DatetimePickerToDate();
            DateTime eDate = searchObj.enddate.DatetimePickerToDate();
            //StartDate > EndDate
            if (DateTime.Compare(sDate, eDate) == 1)
            {
                throw new Exception("รูปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }

            BaseResponse<List<AuditReportResponseDTO>> resAuditReport = await _reportAPI.GetAuditReportAsync(new AuditReportQuery
            {
                transaction_startdate = sDate,
                transaction_enddate = eDate
            });
            if (!resAuditReport.result)
            {
                return Json(new { result = false, message = resAuditReport.error.error.message, data = new List<AuditReportResponseDTO>() });
            }

            return Json(new { result = true, message = "สำเร็จ", data = resAuditReport.data });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<AuditReportResponseDTO>() });
        }
    }

    [Obsolete("*** Move to AuditSaleSummaryReportByBranch(int branchid)")]
    public async Task<IActionResult> AuditSaleSummaryReportByTransaction(int transactionid)
    {
        BaseResponse<SaleSummaryReportResponseDTO> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportByTransIDAsync(transactionid);
        AuditSaleSummaryReportViewModel auditReportViewData = _mapper.Map<AuditSaleSummaryReportViewModel>(resSaleSummaryReport.data);
        return View(auditReportViewData);
    }

    public async Task<IActionResult> AuditSaleSummaryReportByBranch(int branchid, string txndate)
    {
        string sTxnDate = $"{txndate.Substring(0, 2)}/{txndate.Substring(2, 2)}/{txndate.Substring(4, 4)}";
        BaseResponse<SaleSummaryReportResponseDTO> resSaleSummaryReport = await _reportAPI.GetSaleSummaryReportByBranchAsync(new SaleSummaryReportByBranchQuery
        {
            branchid = branchid,
            transactiondate = sTxnDate.ToDate()
        });
        AuditSaleSummaryReportViewModel auditReportViewData = _mapper.Map<AuditSaleSummaryReportViewModel>(resSaleSummaryReport.data);
        auditReportViewData.TransactionDate = resSaleSummaryReport.data.transactiondate.ToDateString();
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
            transactiondatetime = reportData.TransactionDate.ToDate(),
            //createddate = $"{reportData.TransactionDate} {DateTime.Now:HH}:{DateTime.Now:mm}:{DateTime.Now:ss}".ToDateTime(),
            createddate = DateTime.Now
        };
    }

    public async Task<List<SelectListItem>> PrepareSelectBranch()
    {
        var resBranch = await _branchAPI.GetBranchListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }

    public async Task<IActionResult> InventoryReport()
    {
        BaseResponse<List<InventoryReportResponseDTO>> resData = await _reportAPI.GetInventoryReportAsync(new InventoryReportQuery
        {
            reportdate = DateTime.Now
        });
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetInventoryReport()
    {
        try
        {
            #region Paging
            //var form = Request.Form.ToList();
            //string draw = form.Where(w => w.Key == "draw").FirstOrDefault().Value[0];
            //var start = form.Where(w => w.Key == "start").FirstOrDefault().Value[0];
            //var length = form.Where(w => w.Key == "length").FirstOrDefault().Value[0];

            //int pageSize = length != null ? Convert.ToInt32(length) : 0;
            //int skip = start != null ? Convert.ToInt32(start) : 0;
            #endregion

            BaseResponse<List<InventoryReportResponseDTO>> resInventoryReport = await _reportAPI.GetInventoryReportAsync(new InventoryReportQuery
            {
                searchtype = 1,
                reportdate = DateTime.Now
            });
            if (!resInventoryReport.result)
            {
                throw new Exception(resInventoryReport.error.error.message);
            }
            return Json(new { data = resInventoryReport.data });
        }
        catch
        {
            return Json(new { data = new List<AuditReportResponseDTO>() });
        }
    }

    //[Route("report/searchinventoryreport")]
    [HttpPost]
    public async Task<IActionResult> SearchInventoryReportByCriteria([FromBody] SearchInventoryReportModel searchObj)
    {
        try
        {
            #region Paging
            //var form = Request.Form.ToList();
            //string draw = form.Where(w => w.Key == "draw").FirstOrDefault().Value[0];
            //var start = form.Where(w => w.Key == "start").FirstOrDefault().Value[0];
            //var length = form.Where(w => w.Key == "length").FirstOrDefault().Value[0];

            //int pageSize = length != null ? Convert.ToInt32(length) : 0;
            //int skip = start != null ? Convert.ToInt32(start) : 0;
            #endregion

            DateTime sDate = searchObj.searchtype == 1 ? searchObj.reportinventorydate.DatetimePickerToDate() 
                : searchObj.reportinventorydate.DatetimePickerToMonthYear();
            BaseResponse<List<InventoryReportResponseDTO>> resInventoryReport = await _reportAPI.GetInventoryReportAsync(new InventoryReportQuery
            {
                searchtype = searchObj.searchtype,
                reportdate = sDate
            });
            if (!resInventoryReport.result)
            {
                return Json(new { result = false, message = resInventoryReport.error.error.message, data = new List<InventoryReportResponseDTO>() });
            }
            return Json(new { result = true, message = "สำเร็จ", data = resInventoryReport.data });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<AuditReportResponseDTO>() });
        }
    }

    [HttpPost]
    [Consumes("application/json")]
    public IActionResult PostDataTest([FromBody] SearchInventoryReportModel searchObj)
    {
        return Json(new { result = true, message = $"Success", data = searchObj });
    }

    [HttpPost]
    [Consumes("application/json")]
    public IActionResult PostDataTestV2([FromForm] SearchInventoryReportModel searchObj)
    {
        #region Paging
        var form = Request.Form.ToList();
        string draw = form.Where(w => w.Key == "draw").FirstOrDefault().Value[0];
        var start = form.Where(w => w.Key == "start").FirstOrDefault().Value[0];
        var length = form.Where(w => w.Key == "length").FirstOrDefault().Value[0];

        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        int skip = start != null ? Convert.ToInt32(start) : 0;
        #endregion

        return Json(new { result = true, message = $"Success", data = searchObj });
    }

    public async Task<IActionResult> CountStockReportAsync()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.SubItemTypeList = await PrepareSelectSubItemType();

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetCountStockReportV1()
    {
        try
        {
            int dayInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            BaseResponse<List<CountStockReportResponseDTO>> resCountstockReport = await _reportAPI.GetCountStockReportAsync(new CountStockReportQuery
            {
                branchid = null,
                startdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                enddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayInMonth),
                subitemtypeid = null
            });
            if (!resCountstockReport.result)
            {
                throw new Exception(resCountstockReport.error.error.message);
            }
            return Json(new { data = resCountstockReport.data });
        }
        catch
        {
            return Json(new { data = new List<CountStockReportResponseDTO>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SearchCountStockReport([FromBody] SearchCountStockReportViewModel searchObj)
    {
        try
        {
            DateTime? sDate = !string.IsNullOrEmpty(searchObj.startdate) ? searchObj.startdate.DatetimePickerToDate() : null;
            DateTime? eDate = !string.IsNullOrEmpty(searchObj.enddate) ? searchObj.enddate.DatetimePickerToDate() : null;
            //StartDate > EndDate
            if ((sDate.HasValue && eDate.HasValue) 
                && DateTime.Compare(sDate.Value, eDate.Value) == 1)
            {
                throw new Exception("รูปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            BaseResponse<List<CountStockReportResponseDTO>> resCountstockReport = await _reportAPI.GetCountStockReportAsync(new CountStockReportQuery
            {
                branchid = searchObj.branchid,
                startdate = sDate,
                enddate = eDate,
                subitemtypeid = null
            });
            if (!resCountstockReport.result)
            {
                return Json(new { result = false, message = resCountstockReport.error.error.message, data = new List<CountStockReportResponseDTO>() });
            }
            return Json(new { result = true, message = "สำเร็จ", data = resCountstockReport.data });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<CountStockReportResponseDTO>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetCountStockReportV2([FromBody] SearchCountStockReportViewModel searchItem)
    {
        BaseResponse<List<CountStockReportResponseDTO>> countstockData = new BaseResponse<List<CountStockReportResponseDTO>> { data = new List<CountStockReportResponseDTO>() };
        try
        {
            #region Prepare Search Start & End Date
            DateTime? stockStartDate = null;
            DateTime? stockEndDate = null;
            int? branchID = null;
            if (!string.IsNullOrEmpty(searchItem.startdate))
            {
                string[] sTransferDate = searchItem.startdate.Split("-");
                if (sTransferDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                stockStartDate = new DateTime(sTransferDate[2].ToInt32(), sTransferDate[1].ToInt32(), sTransferDate[0].ToInt32());
            }

            if (!string.IsNullOrEmpty(searchItem.enddate))
            {
                string[] sTransferEndDate = searchItem.enddate.Split("-");
                if (sTransferEndDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                stockEndDate = new DateTime(sTransferEndDate[2].ToInt32(), sTransferEndDate[1].ToInt32(), sTransferEndDate[0].ToInt32());
            }

            //เช็ควันที่สิ้นสุดน้อยกว่า วันเริ่มต้น
            if ((stockStartDate.HasValue && stockEndDate.HasValue)
                && DateTime.Compare(stockStartDate.Value, stockEndDate.Value) == 1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            #endregion

            //Set branchid & transfer status
            branchID = searchItem.branchid == 999 ? null : searchItem.branchid;
            if (base.UserProfile.roleid == (int)UserRole.Admin || base.UserProfile.roleid == (int)UserRole.Stock)
            {
                countstockData = await _reportAPI.GetCountStockReportAsync(new CountStockReportQuery
                {
                    branchid = branchID,
                    startdate = stockStartDate,
                    enddate = stockEndDate,
                    subitemtypeid = null
                });
            }
            else
            {
                countstockData = await _reportAPI.GetCountStockReportAsync(new CountStockReportQuery
                {
                    branchid = base.UserProfile.access_branch.FirstOrDefault().branchid,
                    startdate = stockStartDate,
                    enddate = stockEndDate,
                    subitemtypeid = null
                });
            }

            if (!countstockData.result)
            {
                return Json(new { data = new List<InquiryCountStockResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            if (!string.IsNullOrEmpty(searchItem.searchValue))
            {
                string searchValue = searchItem.searchValue.Replace("\t", "").Replace("\n", "");
                countstockData.data = countstockData.data.Where(w => w.branchname.Contains(searchValue)
                || (!string.IsNullOrEmpty(w.remark) ? w.remark.Contains(searchValue) : false)
                || w.subitemtypename.Contains(searchValue)
                || w.createdby.Contains(searchValue)).ToList();
            }
            #endregion

            var totalItems = countstockData.data.Count; // Get total item count for pagination

            // Filter based on searchValue if necessary
            var query = countstockData.data;

            // Calculate paginated data
            //var items = query.Skip(searchItem.start).Take(searchItem.length).ToList();
            var items = query.Skip(0).Take(searchItem.length).ToList();

            // Prepare response for DataTables
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = query.Count(), // Total records after applying filtering
                data = items // The actual data to be displayed
            });
        }
        catch
        {
            // Handle error
            return Json(new { data = new List<InquiryCountStockResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }

    private async Task<List<SelectListItem>> PrepareSelectSubItemType()
    {
        BaseResponse<List<GetSubItemTypeResponseDTO>> resData = await _subItemTypeAPI.GetSubItemTypeListAsync();
        return resData.data.Select(s => new SelectListItem { Text = s.subitemcode, Value = s.subitemtypeid.ToString() }).ToList();
    }
}
