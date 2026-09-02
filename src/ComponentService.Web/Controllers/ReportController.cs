using System.Collections.Generic;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.CountStockAPI;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.ExternalService.ReportAPI;
using CYRetailIMS.Application.ExternalService.SubItemTypeAPI;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReport.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReportByID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockByBrachReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.CountStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemTransferShortageReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.TransactionDeletionLogReport.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Infrastructure.Common.Extensions;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

public class ReportController : BaseController
{
    private readonly IReportAPI _reportAPI;
    private readonly ICountStockAPI _countStockAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly ISubItemTypeAPI _subItemTypeAPI;
    private readonly IItemBrandAPI _itemBrandAPI;

    public ReportController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IReportAPI reportAPI, ICountStockAPI countStockAPI, IBranchAPI branchAPI,
        ISubItemTypeAPI subItemTypeAPI,
        IItemBrandAPI itemBrandAPI) : base(httpClientRequest, mapper, log)
    {
        _reportAPI = reportAPI;
        _countStockAPI = countStockAPI;
        _branchAPI = branchAPI;
        _subItemTypeAPI = subItemTypeAPI;
        _itemBrandAPI = itemBrandAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region Main Action

    [CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.SaleArea)]
    public async Task<IActionResult> SaleReport()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    [CustomAuthorize(RoleName.Admin)]
    public async Task<IActionResult> SaleItemGroupReport()
    {
        var itemBrandList = await PrepareSelectItemBrand();
        var branchList = await PrepareSelectBranch();
        branchList.RemoveAt(0);
        ViewBag.BranchList = branchList;
        ViewBag.ItemBrandList = itemBrandList;
        return View();
    }

    /// <summary>
    /// สรุปยอดรวมประจำวัน ของแต่ละสาขา 1 สาขามี 1 รายการ /1วัน
    /// </summary>
    /// <returns></returns>
    [CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.SaleArea)]
    public async Task<IActionResult> SaleSummaryReport()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    /// <summary>
    /// สรุปยอดรวมประจำวัน ของทุกสาขา 1รายการ/1วัน 
    /// รายงานตั้งแต่วันที่ 1 ของเดือน ถึง end of month
    /// </summary>
    /// <returns></returns>
    [CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.SaleArea)]
    public IActionResult AuditReport()
    {
        return View();
    }


    /// <summary>
    /// รายงานแสดงสินค้าขั้นต่ำ
    /// </summary>
    /// <returns></returns>
    [CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.SaleArea)]
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
    [CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.SaleArea)]
    public async Task<IActionResult> ItemTransactionReport()
    {
        BaseResponse<List<GetBranchResponseDTO>> resBranchList = await _branchAPI.GetBranchListAsync();
        ViewBag.BranchList = resBranchList;
        return View();
    }

    [CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.SaleArea)]
    public async Task<IActionResult> InventoryReport()
    {
        BaseResponse<List<InventoryReportResponseDTO>> resData = await _reportAPI.GetInventoryReportAsync(new InventoryReportQuery
        {
            reportdate = DateTime.Now
        });
        return View();
    }

    /// <summary>
    /// รายงานนับสต๊อก
    /// </summary>
    /// <returns></returns>
    [CustomAuthorize(RoleName.Admin, RoleName.Audit)]
    public async Task<IActionResult> CountStockReportAsync()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.SubItemTypeList = await PrepareSelectSubItemType();

        return View();
    }

    /// <summary>
    /// รายงานประวัติการอนุมัตินับสต๊อก (เฉพาะ Admin)
    /// </summary>
    [CustomAuthorize(RoleName.Admin)]
    public async Task<IActionResult> CountStockApprovalReport()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View("~/Views/Stock/CountStockApprovalReport.cshtml");
    }

    /// <summary>
    /// รายละเอียดราย transaction ของรายงานอนุมัตินับสต๊อก
    /// </summary>
    [CustomAuthorize(RoleName.Admin)]
    public async Task<IActionResult> CountStockApprovalReportDetail(int countstockid)
    {
        var result = await _countStockAPI.GetCountStockApprovalReportByIDAsync(new GetCountStockApprovalReportByIDQuery
        {
            countstockid = countstockid
        });

        if (!result.result || result.data == null)
        {
            return RedirectToAction(nameof(CountStockApprovalReport));
        }

        return View("~/Views/Stock/CountStockApprovalReportDetail.cshtml", result.data);
    }

    /// <summary>
    /// รายงานสินค้าโอนขาด
    /// </summary>
    /// <returns></returns>
    [CustomAuthorize(RoleName.Admin, RoleName.AccountingOfficer, RoleName.SaleArea)]
    public async Task<IActionResult> ItemTransferShortageReport()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.SubItemTypeList = await PrepareSelectSubItemType();
        return View();
    }
    #endregion

    [HttpPost]
    public async Task<IActionResult> SearchSaleReportV2([FromBody] SearchSaleReportViewModel searchItem)
    {
        BaseResponse<List<SaleReportResponseDetailDTO>> resSaleReport = new BaseResponse<List<SaleReportResponseDetailDTO>>();
        try
        {
            #region Prepare Search Start & End Date
            DateTime sDate = DateTime.Now;
            DateTime eDate = DateTime.Now;
            int? branchID = null;

            if (!string.IsNullOrEmpty(searchItem.startdate))
            {
                string[] sTransferDate = searchItem.startdate.Split("-");
                if (sTransferDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                sDate = new DateTime(sTransferDate[2].ToInt32(), sTransferDate[1].ToInt32(), sTransferDate[0].ToInt32());
            }

            if (!string.IsNullOrEmpty(searchItem.enddate))
            {
                string[] sTransferEndDate = searchItem.enddate.Split("-");
                if (sTransferEndDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                eDate = new DateTime(sTransferEndDate[2].ToInt32(), sTransferEndDate[1].ToInt32(), sTransferEndDate[0].ToInt32());
            }

            //เช็ควันที่สิ้นสุดน้อยกว่า วันเริ่มต้น
            if (DateTime.Compare(sDate, eDate) == 1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            #endregion

            branchID = searchItem.branchid == 999 ? null : searchItem.branchid;
            BaseResponse<SaleReportResponseDTO> resReport = await _reportAPI.GetSaleReportByCriteriaAsync(new SaleReportQuery
            {
                transaction_startdate = sDate,
                transaction_enddate = eDate,
                branchid = branchID,
                startrow = searchItem.start,
                pagesize = searchItem.length,
                //searchvalue = searchItem.searchValue.Replace("\t", "").Replace("\n", ""),
                isexportalldata = searchItem.isexportalldata,
            });

            if (!resReport.result)
            {
                return Json(new { data = new List<SaleReportResponseDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            if (!string.IsNullOrEmpty(searchItem.searchValue))
            {
                string searchValue = searchItem.searchValue.Replace("\t", "").Replace("\n", "");
                
                resReport.data.transactiondata = resReport.data.transactiondata.Where(w => w.itemname.Contains(searchValue)
                || w.itemcode.Contains(searchValue)
                || w.branchname.Contains(searchValue)
                || w.brandname.Contains(searchValue)
                || w.createdby.Contains(searchValue)).ToList();
            }
            #endregion

            //var totalRows = resReport.data.totalrow;
            var totalItems = resReport.data.totalrow; // Get total item count for pagination

            // Filter based on searchValue if necessary
            var query = resReport.data.transactiondata;

            // Calculate paginated data
            //var items = searchItem.isexportalldata ? query : query.Skip(searchItem.start).Take(searchItem.length).ToList();
            
            // Prepare response for DataTables
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = totalItems, // Total records after applying filtering
                data = resReport.data.transactiondata // The actual data to be displayed
            });
        }
        catch
        {
            return Json(new { data = new List<SaleReportResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetCountStockApprovalReport([FromBody] SearchCountStockApprovalReportViewModel? searchItem)
    {
        int draw = searchItem?.draw ?? 1;

        if (searchItem == null)
        {
            return Json(new { draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });
        }

        try
        {
            DateTime? approvedStart = !string.IsNullOrWhiteSpace(searchItem.startdate) ? searchItem.startdate.DatetimePickerToDate() : null;
            DateTime? approvedEnd = !string.IsNullOrWhiteSpace(searchItem.enddate) ? searchItem.enddate.DatetimePickerToDate() : null;

            var result = await _countStockAPI.GetCountStockApprovalReportAsync(new GetCountStockApprovalReportQuery
            {
                branchid = searchItem.branchid,
                startdate = approvedStart,
                enddate = approvedEnd,
                startrow = searchItem.start,
                pagesize = searchItem.length,
                searchvalue = searchItem.searchValue,
                isexportalldata = false
            });

            if (!result.result || result.data == null)
            {
                return Json(new { draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });
            }

            return Json(new
            {
                draw,
                recordsTotal = result.data.totalrow,
                recordsFiltered = result.data.totalrow,
                data = result.data.transactiondata
            });
        }
        catch
        {
            return Json(new { draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });
        }
    }

    [HttpGet]
    [CustomAuthorize(RoleName.Admin)]
    public async Task<IActionResult> ExportCountStockApprovalReportExcel(string? startdate, string? enddate, int? branchid)
    {
        DateTime? approvedStart = !string.IsNullOrWhiteSpace(startdate) ? startdate.DatetimePickerToDate() : null;
        DateTime? approvedEnd = !string.IsNullOrWhiteSpace(enddate) ? enddate.DatetimePickerToDate() : null;

        var result = await _countStockAPI.GetCountStockApprovalReportAsync(new GetCountStockApprovalReportQuery
        {
            branchid = branchid,
            startdate = approvedStart,
            enddate = approvedEnd,
            startrow = 0,
            pagesize = 0,
            searchvalue = null,
            isexportalldata = true
        });

        var reportRows = result.data?.transactiondata ?? new List<GetCountStockApprovalReportItemDTO>();
        if (!result.result || !reportRows.Any())
        {
            return NotFound("ไม่พบข้อมูลรายงานอนุมัตินับสต๊อก");
        }

        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Approval CountStock");

        string[] headers =
        {
            "เลขที่นับสต๊อก", "วันที่นับ", "สาขา", "ผู้นับ", "ผู้อนุมัติ", "วันที่อนุมัติ",
            "จำนวนรายการ", "รวมก่อนปรับ", "รวมหลังปรับ", "ผลต่าง"
        };

        for (int c = 0; c < headers.Length; c++)
        {
            ws.Cells[1, c + 1].Value = headers[c];
        }

        int row = 2;
        foreach (var d in reportRows)
        {
            ws.Cells[row, 1].Value = d.countstockid;
            ws.Cells[row, 2].Value = d.countstockdate;
            ws.Cells[row, 2].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
            ws.Cells[row, 3].Value = d.branchname;
            ws.Cells[row, 4].Value = d.counterrole;
            ws.Cells[row, 5].Value = d.approvedby;
            ws.Cells[row, 6].Value = d.approveddate;
            ws.Cells[row, 6].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
            ws.Cells[row, 7].Value = d.totalitems;
            ws.Cells[row, 8].Value = d.totalqtybefore;
            ws.Cells[row, 9].Value = d.totalqtyafter;
            ws.Cells[row, 10].Value = d.totaladjustedqty;
            row++;
        }

        ws.Cells.AutoFitColumns();
        var bytes = package.GetAsByteArray();
        string fileName = $"CountStockApprovalReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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

    private async Task<List<SelectListItem>> PrepareSelectBranch()
    {
        var resBranch = await _branchAPI.GetBranchListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }

    private async Task<List<SelectListItem>> PrepareSelectItemBrand()
    {
        var resItemBrands = await _itemBrandAPI.GetItemBrandListAsync();
        return resItemBrands.data.Select(s => new SelectListItem { Text = s.brandname, Value = s.brandid.ToString() }).ToList();
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

    #region รายงานนับสต๊อก
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
    #endregion

    private async Task<List<SelectListItem>> PrepareSelectSubItemType()
    {
        BaseResponse<List<GetSubItemTypeResponseDTO>> resData = await _subItemTypeAPI.GetSubItemTypeListAsync();
        return resData.data.Select(s => new SelectListItem { Text = s.subitemcode, Value = s.subitemtypeid.ToString() }).ToList();
    }

    #region รายงานสินค้าโอนขาด

    /// <summary>
    /// Get current month
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetItemTransferShortageReport()
    {
        try
        {
            int dayInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            BaseResponse<List<ItemTransferShortageReportResponseDTO>> resTransferReport = await _reportAPI.GetItemTransferShortageReportAsync(new ItemTransferShortageReportQuery
            {
                branchid = null,
                transferstartdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                transferenddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, dayInMonth),
                subitemtypeid = null
            });
            if (!resTransferReport.result)
            {
                throw new Exception(resTransferReport.error.error.message);
            }
            return Json(new { data = resTransferReport.data });
        }
        catch
        {
            return Json(new { data = new List<ItemTransferShortageReportResponseDTO>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SearchItemTransferShortageReport([FromBody] TransferShortageReportViewModel searchObj)
    {
        try
        {
            DateTime? sDate = !string.IsNullOrEmpty(searchObj.startdate) ? searchObj.startdate.DatetimePickerToDate() : null;
            DateTime? eDate = !string.IsNullOrEmpty(searchObj.enddate) ? searchObj.enddate.DatetimePickerToDate() : null;
  
            if ((sDate.HasValue && eDate.HasValue)
                && DateTime.Compare(sDate.Value, eDate.Value) == 1)
            {
                throw new Exception("รูปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            BaseResponse<List<ItemTransferShortageReportResponseDTO>> resCountstockReport = await _reportAPI.GetItemTransferShortageReportAsync(new ItemTransferShortageReportQuery
            {
                branchid = searchObj.branchid,
                transferstartdate = sDate,
                transferenddate = eDate,
                subitemtypeid = searchObj.subitemtypeid
            });
            if (!resCountstockReport.result)
            {
                return Json(new { result = false, message = resCountstockReport.error.error.message, data = new List<ItemTransferShortageReportResponseDTO>() });
            }
            return Json(new { result = true, message = "สำเร็จ", data = resCountstockReport.data });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย, เกิดข้อผิดพลาด {ex.Message}", data = new List<ItemTransferShortageReportResponseDTO>() });
        }
    }
    #endregion

    #region รายงานสต๊อกสินค้า
    [CustomAuthorize(RoleName.Admin)]
    public async Task<IActionResult> ItemStockReport()
    {
        var branchList = await PrepareSelectBranch();
        branchList.Insert(0, new SelectListItem { Text = "ทุกสาขา", Value = "" });
        ViewBag.BranchList = branchList;
        ViewBag.SubItemTypeList = await PrepareSelectSubItemType();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SearchItemStockReport([FromBody] SearchItemStockReportViewModel searchItem)
    {
        //BaseResponse<ItemStockReportResponseDTO> resItemStockReport = new BaseResponse<ItemStockReportResponseDTO>();
        try
        {
            #region Prepare Search Start & End Date
            DateTime sDate = DateTime.Now;
            DateTime eDate = DateTime.Now;
            int? branchID = null;

            if (!string.IsNullOrEmpty(searchItem.startdate))
            {
                string[] sTransferDate = searchItem.startdate.Split("-");
                if (sTransferDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                sDate = new DateTime(sTransferDate[2].ToInt32(), sTransferDate[1].ToInt32(), sTransferDate[0].ToInt32());
            }

            if (!string.IsNullOrEmpty(searchItem.enddate))
            {
                string[] sTransferEndDate = searchItem.enddate.Split("-");
                if (sTransferEndDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                eDate = new DateTime(sTransferEndDate[2].ToInt32(), sTransferEndDate[1].ToInt32(), sTransferEndDate[0].ToInt32());
            }

            //เช็ควันที่สิ้นสุดน้อยกว่า วันเริ่มต้น
            if (DateTime.Compare(sDate, eDate) == 1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            #endregion

            branchID = searchItem.branchid == 999 ? null : searchItem.branchid;
            BaseResponse<ItemStockReportResponseDTO> resReport = await _reportAPI.GetItemStockReportAsync(new ItemStockReportQuery
            {
                branchid = branchID,
                startrow = searchItem.start,
                pagesize = searchItem.length,
                //searchvalue = searchItem.searchValue.Replace("\t", "").Replace("\n", ""),
                isexportalldata = searchItem.isexportalldata,
            });

            if (!resReport.result)
            {
                return Json(new { data = new List<ItemStockReportDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            if (!string.IsNullOrEmpty(searchItem.searchValue))
            {
                string searchValue = searchItem.searchValue.Replace("\t", "").Replace("\n", "");

                resReport.data.data = resReport.data.data.Where(w => w.itemname.Contains(searchValue)
                || w.itemcode.Contains(searchValue)
                || w.branchname.Contains(searchValue)
                || w.brandname.Contains(searchValue)
                || w.itemname.Contains(searchValue)
                || w.itemcode.Contains(searchValue)).ToList();
            }
            #endregion

            //var totalRows = resReport.data.totalrow;
            var totalItems = resReport.data.totalrow; // Get total item count for pagination

            #region Search by order: ยังไม่เสร็จ ไม่สามารถ order ทั้งหมดได้ order ได้แค่หน้าปัจจุบัน
            //// Filter based on searchValue if necessary
            //var query = resReport.data.data.AsQueryable();

            //string columnName = searchItem.columns[searchItem.order[0].column].GetColumnName();
            //var orderColumnName = columnName;

            //if (searchItem.order != null && searchItem.order.FirstOrDefault().dir == "asc")
            //{
            //    query = query.OrderByDynamic(propertyName: orderColumnName, ascending: true);
            //}
            //else
            //{
            //    query = query.OrderByDynamic(propertyName: orderColumnName, ascending: false);
            //}
            #endregion

            // Calculate paginated data
            //var items = searchItem.isexportalldata ? query : query.Skip(searchItem.start).Take(searchItem.length).ToList();

            // Prepare response for DataTables
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = totalItems, // Total records after applying filtering
                data = resReport.data.data // The actual data to be displayed
            });
        }
        catch
        {
            return Json(new { data = new List<ItemStockReportDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }

    #endregion

    #region รายงานยอดรวมตามรหัสสินค้า
    [HttpPost]
    public async Task<IActionResult> SearchSaleItemGroupReport([FromBody] SearchSaleReportViewModel searchItem)
    {
        BaseResponse<List<SaleReportGroupByBranchDetailDTO>> resSaleReport = new BaseResponse<List<SaleReportGroupByBranchDetailDTO>>();
        try
        {
            #region Prepare Search Start & End Date
            DateTime sDate = DateTime.Now;
            DateTime eDate = DateTime.Now;
            int? branchID = null;

            if (!string.IsNullOrEmpty(searchItem.startdate))
            {
                string[] sTransferDate = searchItem.startdate.Split("-");
                if (sTransferDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                sDate = new DateTime(sTransferDate[2].ToInt32(), sTransferDate[1].ToInt32(), sTransferDate[0].ToInt32());
            }

            if (!string.IsNullOrEmpty(searchItem.enddate))
            {
                string[] sTransferEndDate = searchItem.enddate.Split("-");
                if (sTransferEndDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                eDate = new DateTime(sTransferEndDate[2].ToInt32(), sTransferEndDate[1].ToInt32(), sTransferEndDate[0].ToInt32());
            }

            //เช็ควันที่สิ้นสุดน้อยกว่า วันเริ่มต้น
            if (DateTime.Compare(sDate, eDate) == 1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            #endregion

            branchID = searchItem.branchid == 999 || searchItem.branchid == 0 ? null : searchItem.branchid;
            BaseResponse<SaleReportGroupByBranchResposneDTO> resReport = await _reportAPI.GetSaleReportByGroupAsync(new SaleReportGroupByBranchQuery
            {
                transaction_startdate = sDate,
                transaction_enddate = eDate,
                branchid = branchID,
                itembrandid = searchItem.itembrandid,
                startrow = searchItem.start,
                pagesize = searchItem.length,
                searchvalue = searchItem.searchValue.Replace("\t", "").Replace("\n", ""),
                isexportalldata = searchItem.isexportalldata,
            });

            if (!resReport.result)
            {
                return Json(new { data = new List<SaleReportGroupByBranchDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            //if (!string.IsNullOrEmpty(searchItem.searchValue))
            //{
            //    string searchValue = searchItem.searchValue.Replace("\t", "").Replace("\n", "");

            //    resReport.data.transactiondata = resReport.data.transactiondata.Where(w => w.itemname.Contains(searchValue)
            //    || w.itemcode.Contains(searchValue)
            //    || w.branchname.Contains(searchValue)
            //    || w.brandname.Contains(searchValue)
            //    || w.createdby.Contains(searchValue)).ToList();
            //}
            #endregion

            //var totalRows = resReport.data.totalrow;
            var totalItems = resReport.data.totalrow; // Get total item count for pagination

            // Filter based on searchValue if necessary
            var query = resReport.data.transactiondata;

            // Calculate paginated data
            //var items = searchItem.isexportalldata ? query : query.Skip(searchItem.start).Take(searchItem.length).ToList();

            // Prepare response for DataTables
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = totalItems, // Total records after applying filtering
                data = resReport.data.transactiondata // The actual data to be displayed
            });
        }
        catch
        {
            return Json(new { data = new List<SaleReportGroupByBranchDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }
    #endregion


    #region [SaleBarcodeReport] รายงานสรุปยอดสิ้นวันบาร์โค้ด
    [CustomAuthorize(RoleName.Admin)]
    public async Task<IActionResult> SaleBarcodeReportAsync()
    {
        var branchList = await PrepareSelectBranch();
        branchList.RemoveAt(0);
        branchList.Insert(0, new SelectListItem { Text = "ทุกสาขา", Value = "" });
        ViewBag.BranchList = branchList;
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> SearchSaleBarcodeReport([FromBody] SearchSaleBarcodeReportViewModel searchObj)
    {
        try
        {
            int? branchID = 0;
            // Default dates to current month if not provided
            DateTime sDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);
            DateTime eDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            if (!string.IsNullOrEmpty(searchObj.startdate))
            {
                sDate = searchObj.startdate.DatetimePickerToDate();
            }
            if (!string.IsNullOrEmpty(searchObj.enddate))
            {
                eDate = searchObj.enddate.DatetimePickerToDate();
            }

            //StartDate > EndDate
            if (DateTime.Compare(sDate, eDate) ==1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            branchID = searchObj.branchid == 999 ? null : searchObj.branchid;
            BaseResponse<SaleBarcodeReportResponseDTO> resReport = await _reportAPI.GetSaleBarcodeReportAsync(new SaleBarcodeReportQuery
            {
                transaction_startdate = sDate,
                transaction_enddate = eDate,
                branchid = branchID,
                startrow = searchObj.start,
                pagesize = searchObj.length,
                searchvalue = searchObj.searchValue.Replace("\t", "").Replace("\n", ""),
                isexportalldata = searchObj.isexportalldata,
            });

            if (!resReport.result)
            {
                return Json(new { data = new List<SaleBarcodeReportResponseDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            if (!string.IsNullOrEmpty(searchObj.searchValue))
            {
                string searchValue = searchObj.searchValue.Replace("\t", "").Replace("\n", "");

                resReport.data.data = resReport.data.data.Where(w => w.branchname.Contains(searchValue)
                || w.auditorname.Contains(searchValue)
                || w.username.Contains(searchValue)).ToList();
            }
            #endregion

            //var totalRows = resReport.data.totalrow;
            var totalItems = resReport.data.totalrow; // Get total item count for pagination

            // Filter based on searchValue if necessary
            //var query = resReport.data.data;

            // Calculate paginated data
            //var items = searchItem.isexportalldata ? query : query.Skip(searchItem.start).Take(searchItem.length).ToList();

            // Prepare response for DataTables
            return Json(new
            {
                draw = searchObj.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = totalItems, // Total records after applying filtering
                data = resReport.data.data // The actual data to be displayed
            });
        }
        catch (Exception ex)
        {
            return Json(new { data = new List<SaleBarcodeReportResponseDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }
    #endregion


    #region [TransactionCancelReport] รายงานยกเลิกรายการขาย
    [CustomAuthorize(RoleName.Admin)]
    public async Task<IActionResult> TransactionCancelReportAsync()
    {
        var branchList = await PrepareSelectBranch();
        branchList.RemoveAt(0);
        ViewBag.BranchList = branchList;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SearchTransactionCancelReport([FromBody] SearchTransactionCanceledLogReportViewModel searchItem)
    {
        try
        {
            #region Prepare Search Start & End Date
            DateTime sDate = DateTime.Now;
            DateTime eDate = DateTime.Now;
            int? branchID = null;

            if (!string.IsNullOrEmpty(searchItem.startdate))
            {
                string[] sTransferDate = searchItem.startdate.Split("-");
                if (sTransferDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                sDate = new DateTime(sTransferDate[2].ToInt32(), sTransferDate[1].ToInt32(), sTransferDate[0].ToInt32());
            }

            if (!string.IsNullOrEmpty(searchItem.enddate))
            {
                string[] sTransferEndDate = searchItem.enddate.Split("-");
                if (sTransferEndDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                eDate = new DateTime(sTransferEndDate[2].ToInt32(), sTransferEndDate[1].ToInt32(), sTransferEndDate[0].ToInt32());
            }

            //เช็ควันที่สิ้นสุดน้อยกว่า วันเริ่มต้น
            if (DateTime.Compare(sDate, eDate) == 1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            #endregion

            branchID = searchItem.branchid == 999 || searchItem.branchid == 0 ? null : searchItem.branchid;
            BaseResponse<TransactionDeletionLogReportResponseDTO> resReport = await _reportAPI.GetTransactionDeletionLogReportAsync(new TransactionDeletionLogReportQuery
            {
                transaction_startdate = sDate,
                transaction_enddate = eDate,
                branchid = branchID,
                startrow = searchItem.start,
                pagesize = searchItem.length,
                searchvalue = searchItem.searchValue.Replace("\t", "").Replace("\n", ""),
                isexportalldata = searchItem.isexportalldata,
            });

            if (!resReport.result)
            {
                return Json(new { data = new List<TransactionDeletionLogReportDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            //if (!string.IsNullOrEmpty(searchItem.searchValue))
            //{
            //    string searchValue = searchItem.searchValue.Replace("\t", "").Replace("\n", "");

            //    resReport.data.transactiondata = resReport.data.transactiondata.Where(w => w.itemname.Contains(searchValue)
            //    || w.itemcode.Contains(searchValue)
            //    || w.branchname.Contains(searchValue)
            //    || w.brandname.Contains(searchValue)
            //    || w.createdby.Contains(searchValue)).ToList();
            //}
            #endregion

            //var totalRows = resReport.data.totalrow;
            var totalItems = resReport.data.totalrow; // Get total item count for pagination

            // Filter based on searchValue if necessary
            var query = resReport.data.transactiondata;

            // Calculate paginated data
            //var items = searchItem.isexportalldata ? query : query.Skip(searchItem.start).Take(searchItem.length).ToList();

            // Prepare response for DataTables
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = totalItems, // Total records after applying filtering
                data = resReport.data.transactiondata // The actual data to be displayed
            });
        }
        catch
        {
            return Json(new { data = new List<TransactionDeletionLogReportDetailDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }
    #endregion
}
