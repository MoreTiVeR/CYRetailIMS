using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.CountStockAPI;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using Microsoft.AspNetCore.Mvc.Rendering;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.ExternalService.SubItemTypeAPI;
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
using Microsoft.EntityFrameworkCore;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.UpdateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.DeleteCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.SubmitCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.ApproveCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetPendingApprovals.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockComparison.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReport.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReportByID.v1;
using System.Globalization;
using OfficeOpenXml;
using CreateCountStockCommandV1 = CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1.CreateCountStockCommand;
using CreateCountStockDetailV1 = CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1.CreateCountStockDetail;
using CreateCountStockCommandV2 = CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v2.CreateCountStockCommand;
using CreateCountStockDetailV2 = CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v2.CreateCountStockDetail;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.SaleArea, RoleName.Stock, RoleName.Sale)]
public class StockController : BaseController
{
    private readonly ICountStockAPI _countStockAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IItemInBranchAPI _itemInBranchAPI;
    private readonly ISubItemTypeAPI _subItemTypeAPI;
    private readonly IItemTypeAPI _itemTypeAPI;

    public StockController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        ICountStockAPI countStockAPI,
        IBranchAPI branchAPI,
        IItemInBranchAPI itemInBranchAPI,
        ISubItemTypeAPI subItemTypeAPI,
        IItemTypeAPI itemTypeAPI) : base(httpClientRequest, mapper, log)
    {
        _countStockAPI = countStockAPI;
        _branchAPI = branchAPI;
        _itemInBranchAPI = itemInBranchAPI;
        _subItemTypeAPI = subItemTypeAPI;
        _itemTypeAPI = itemTypeAPI;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public async Task<IActionResult> CountStockAsync()
    {
        // Mock data for demonstration
        //var items = await _countStockAPI.InquiryCountStockByBranchIDAsync(new InquiryCountStockByBranchIDQuery
        //{
        //    branchid = base.UserProfile.roleid == (int)EnumModel.UserRole.Admin ? 0 : base.UserProfile.access_branch.FirstOrDefault().branchid
        //});

        ViewBag.ItemTypeList = await PrepareSelectItemType();
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }


    public async Task<IActionResult> Edit(int cstockid)
    {
        BaseResponse<InquiryCountStockByIDResponseDTO> resCountStockData = await _countStockAPI.InquiryCountStockByStockIDAsync(new InquiryCountStockByIDQuery
        {
            countstockid = cstockid
        });
        ViewBag.ItemTypeList = await PrepareSelectItemType();
        //ViewBag.BranchList = await PrepareSelectBranch();
        var BranchList = new List<SelectListItem>();
        BranchList.Add(new SelectListItem
        {
            Text = resCountStockData.data.branchname,
            Value = resCountStockData.data.branchid.ToString()
        });
        ViewBag.BranchList = BranchList;
        return View(resCountStockData.data);
    }

    #region Http Method

    [HttpPost]
    public async Task<IActionResult> GetCountStocks([FromBody] SearchCountStockViewModel searchItem)
    {
        BaseResponse<List<InquiryCountStockResponseDTO>> countstockData = new BaseResponse<List<InquiryCountStockResponseDTO>> { data = new List<InquiryCountStockResponseDTO>() };
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
                countstockData = await _countStockAPI.GetCountStockListAsync(new InquiryCountStocksQuery 
                { 
                    branchid = branchID,
                    startdate = stockStartDate,
                    enddate = stockEndDate
                });
            }
            else
            {
                countstockData = await _countStockAPI.GetCountStockListAsync(new InquiryCountStocksQuery 
                {
                    branchid = base.UserProfile.access_branch.FirstOrDefault().branchid,
                    startdate = stockStartDate,
                    enddate = stockEndDate
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
            var items = query.Skip(searchItem.start).Take(searchItem.length).ToList();

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

    [HttpPost]
    public async Task<IActionResult> GetStockDataByBranch([FromBody] SearchItemViewModel searchItem)
    {
        // Fetch the data based on the branchId
        var stockData = await _countStockAPI.InquiryCountStockByBranchIDAsync(new InquiryCountStockByBranchIDQuery
        {
            branchid = searchItem.branchid
        });
        if (!stockData.result)
        {
            return Json(new { result = false, message = stockData.error.error.message, data = new List<InquiryCountStockByBranchIDResponseDTO>() });
        }

        // Return the data as JSON
        return Json(new { result = true, message = "สำเร็จ", data = stockData.data });
    }

    /// <summary>
    /// โหลดข้อมูลสต๊อกระดับรายสินค้า (รหัสสินค้า/ชื่อสินค้า/ประเภทย่อย) สำหรับหน้านับสต๊อกแบบใหม่
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> GetItemStockDataByBranch([FromBody] SearchItemViewModel searchItem)
    {
        var stockData = await _countStockAPI.InquiryCountStockByBranchIDAsync(new InquiryCountStockByBranchIDQuery
        {
            branchid = searchItem.branchid,
            itemlevel = true
        });
        if (!stockData.result)
        {
            return Json(new { result = false, message = stockData.error.error.message, data = new List<InquiryCountStockByBranchIDResponseDTO>() });
        }

        var data = stockData.data ?? new List<InquiryCountStockByBranchIDResponseDTO>();
        bool hasMissingItemInfo = data.Any(w => string.IsNullOrWhiteSpace(w.itemcode) || string.IsNullOrWhiteSpace(w.itemname));

        if (hasMissingItemInfo)
        {
            var itemInBranchRes = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(searchItem.branchid);
            if (itemInBranchRes.result && itemInBranchRes.data?.itemlist != null)
            {
                var itemLookup = itemInBranchRes.data.itemlist
                    .GroupBy(g => g.itemid)
                    .ToDictionary(
                        k => k.Key,
                        v => v.FirstOrDefault());

                foreach (var row in data)
                {
                    if (!itemLookup.TryGetValue(row.itemid, out var itemRef) || itemRef == null)
                    {
                        continue;
                    }

                    if (row.subitemtypeid <= 0 && itemRef.subitemtypeid.HasValue && itemRef.subitemtypeid.Value > 0)
                    {
                        row.subitemtypeid = itemRef.subitemtypeid.Value;
                    }

                    if (string.IsNullOrWhiteSpace(row.itemcode))
                    {
                        row.itemcode = itemRef.itemcode;
                    }

                    if (string.IsNullOrWhiteSpace(row.itemname))
                    {
                        row.itemname = itemRef.itemname;
                    }

                    if (string.IsNullOrWhiteSpace(row.subitemcode) && !string.IsNullOrWhiteSpace(itemRef.subitemtypename))
                    {
                        row.subitemcode = itemRef.subitemtypename;
                    }
                }
            }
        }

        string currentCounterRole = base.UserProfile.roleid == (int)UserRole.SaleArea ? "HeadPC" : "PC";
        DateTime today = DateTime.Today;
        DateTime tomorrow = today.AddDays(1);

        var pendingDrafts = await _countStockAPI.GetPendingApprovalsAsync(new GetPendingApprovalsQuery
        {
            counterrole = currentCounterRole,
            statuscid = 0
        });

        if (pendingDrafts.result && pendingDrafts.data != null)
        {
            var latestDraft = pendingDrafts.data
                .Where(w => w.branchid == searchItem.branchid
                            && w.countstockdate >= today
                            && w.countstockdate < tomorrow)
                .OrderByDescending(o => o.countstockdate)
                .ThenByDescending(o => o.countstockid)
                .FirstOrDefault();

            if (latestDraft != null)
            {
                var draftDetailRes = await _countStockAPI.InquiryCountStockByStockIDAsync(new InquiryCountStockByIDQuery
                {
                    countstockid = latestDraft.countstockid
                });

                if (draftDetailRes.result && draftDetailRes.data?.detail != null)
                {
                    var draftDetailLookup = draftDetailRes.data.detail
                        .GroupBy(g => new { g.itemid, g.subitemtypeid })
                        .ToDictionary(k => k.Key, v => v.FirstOrDefault());

                    foreach (var row in data)
                    {
                        var draftKey = new { itemid = row.itemid, subitemtypeid = row.subitemtypeid };
                        if (!draftDetailLookup.TryGetValue(draftKey, out var draftDetail) || draftDetail == null)
                        {
                            continue;
                        }

                        row.countedqty = draftDetail.countedqty;
                        row.waitingtorestock = draftDetail.waitingtorestock;
                        row.damaged = draftDetail.damaged;
                        row.soldbeforecount = draftDetail.soldbeforecount;
                        row.totalcounted = draftDetail.totalcounted;
                        row.difference = draftDetail.difference;
                        row.itemremark = draftDetail.itemremark;
                    }
                }
            }
        }

        return Json(new { result = true, message = "สำเร็จ", data });
    }

    [HttpPost]
    public async Task<IActionResult> GetStockDataByCountStockID([FromBody] SearchCountStockByIDViewModel searchItem)
    {
        // Fetch the data based on the branchId
        BaseResponse<InquiryCountStockByIDResponseDTO> resCountStockData = await _countStockAPI.InquiryCountStockByStockIDAsync(new InquiryCountStockByIDQuery
        {
            countstockid = searchItem.countstockid
        });
        if (!resCountStockData.result)
        {
            return Json(new { result = false, message = resCountStockData.error.error.message, data = new List<InquiryCountStockByBranchIDResponseDTO>() });
        }

        // Return the data as JSON
        return Json(new { result = true, message = "สำเร็จ", data = resCountStockData.data.detail });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCountStock([FromBody] List<CountStockCreateModel> updatedItems)
    {
        try
        {
            if (!updatedItems.Any())
            {
                return Json(new { result = false, message = "ไม่พบรายการนับสต๊อก กรุณาเลือกขาสาเพื่อทำรายการ" });
            }
            CreateCountStockCommandV1 countStockCommand = PrepareCreateCountStockData(updatedItems);
            var resCreate = await _countStockAPI.CreateCountStockListAsync(countStockCommand);
            if (!resCreate.result)
            {
                return Json(new { result = false, message = "ข้อมูลนับสต๊อกไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง" });
            }
            return Json(new { result = true, message = "ทำรายการสำเร็จ." });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย มีบางอย่างผิดพลาด กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCountStock([FromBody] List<CountStockUpdateModel> updatedItems)
    {
        try
        {
            if(updatedItems.Any(w => w.CountStockDetailID == 0))
            {
                return Json(new { result = false, message = "รายการแก้ไขไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง" });
            }
            if (!updatedItems.Any())
            {
                return Json(new { result = false, message = "ไม่พบรายการนับสต๊อก กรุณาเลือกขาสาเพื่อทำรายการ" });
            }
            UpdateCountStockCommand countStockCommand = PrepareUpdateCountStockData(updatedItems);
            var resUpdate = await _countStockAPI.UpdateCountStocAsync(countStockCommand);
            if (!resUpdate.result)
            {
                return Json(new { result = false, message = resUpdate.error.error.message });
            }
            return Json(new { result = true, message = "ปรับปรุงข้อมูลนับสต๊อกสำเร็จ." });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย มีบางอย่างผิดพลาด กรุณาลองใหม่อีกครั้ง!. {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCountStockAsync([FromBody] DeleteCountStockModel deleteCountStock)
    {
        try
        {
            var resUpdate = await _countStockAPI.DeleteCountStockAsync(new DeleteCountStockCommand
            {
                countstockid = deleteCountStock.countstockid,
                deletedby = base.UserProfile.username
            });
            if (!resUpdate.result)
            {
                return Json(new { result = false, message = resUpdate.error.error.message });
            }
            return Json(new { result = true, message = "ลบข้อมูลสำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย เกิดข้อผิดพลาด: {ex.Message}" });
        }
    }
    #endregion

    #region New Count Stock (v2) Actions — หน้านับสต๊อกแบบใหม่

    /// <summary>
    /// หน้านับสต๊อก (แบบใหม่) — PC และ HeadPC กรอกข้อมูลนับสต๊อก
    /// </summary>
    [CustomAuthorize(RoleName.Sale, RoleName.SaleArea)]
    public async Task<IActionResult> NewCountStockEntry()
    {
        ViewBag.ItemTypeList = await PrepareSelectSubItemType();
        ViewBag.BranchList = await PrepareSelectBranch();
        // Pass current user's role to view so JS can adjust UI
        ViewBag.CounterRole = base.UserProfile.roleid == (int)UserRole.SaleArea ? "HeadPC" : "PC";
        return View();
    }

    /// <summary>
    /// หน้าเทียบข้อมูล — เปรียบเทียบสต๊อกระบบกับยอดที่นับได้
    /// </summary>
    [CustomAuthorize(RoleName.Admin, RoleName.Stock, RoleName.SaleArea, RoleName.Sale)]
    public async Task<IActionResult> CountStockCompare()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        ViewBag.ItemTypeList = await PrepareSelectSubItemType();
        return View();
    }

    /// <summary>
    /// หน้ารออนุมัติ — รายการที่ส่งมารออนุมัติ, Admin กดอนุมัติได้เฉพาะรายการ HeadPC
    /// </summary>
    [CustomAuthorize(RoleName.Admin, RoleName.SaleArea)]
    public IActionResult CountStockPendingApproval()
    {
        ViewBag.IsAdmin = base.UserProfile.roleid == (int)UserRole.Admin;
        return View();
    }

    /// <summary>
    /// รายงานประวัติการอนุมัตินับสต๊อก (เฉพาะ Admin)
    /// </summary>
    [CustomAuthorize(RoleName.Admin)]
    public IActionResult CountStockApprovalReport()
    {
        return RedirectToAction("CountStockApprovalReport", "Report");
    }

    /// <summary>
    /// รายละเอียดราย transaction ของรายงานอนุมัตินับสต๊อก
    /// </summary>
    [CustomAuthorize(RoleName.Admin)]
    public IActionResult CountStockApprovalReportDetail(int countstockid)
    {
        return RedirectToAction("CountStockApprovalReportDetail", "Report", new { countstockid });
    }

    #endregion

    #region New Count Stock HTTP Methods

    [HttpPost]
    public async Task<IActionResult> GetCountStockComparison([FromBody] SearchCountStockComparisonViewModel searchItem)
    {
        try
        {
            DateTime? salesStart = ParseDate(searchItem.salesstartdate);
            DateTime? salesEnd = ParseDate(searchItem.salesenddate);
            DateTime? auditStart = ParseDate(searchItem.auditstartdate);
            DateTime? auditEnd = ParseDate(searchItem.auditenddate);

            var result = await _countStockAPI.GetCountStockComparisonAsync(new GetCountStockComparisonQuery
            {
                branchid = searchItem.branchid,
                subitemtypename = searchItem.subitemtypename,
                salesstartdate = salesStart,
                salesenddate = salesEnd,
                auditstartdate = auditStart,
                auditenddate = auditEnd
            });

            if (!result.result)
                return Json(new { draw = searchItem.draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });

            var filtered = result.data;
            if (!string.IsNullOrEmpty(searchItem.searchValue))
            {
                string sv = searchItem.searchValue.Trim();
                filtered = filtered.Where(w => w.itemcode.Contains(sv) || w.itemname.Contains(sv) || w.subitemtypename.Contains(sv)).ToList();
            }

            var items = filtered.Skip(searchItem.start).Take(searchItem.length).ToList();
            return Json(new { draw = searchItem.draw, recordsTotal = filtered.Count, recordsFiltered = filtered.Count, data = items });
        }
        catch
        {
            return Json(new { draw = searchItem.draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetPendingApprovals([FromBody] SearchPendingApprovalViewModel searchItem)
    {
        try
        {
            var result = await _countStockAPI.GetPendingApprovalsAsync(new GetPendingApprovalsQuery
            {
                counterrole = searchItem.counterrole,
                statuscid = searchItem.statuscid
            });

            if (!result.result)
                return Json(new { draw = searchItem.draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });

            var filtered = result.data;
            if (!string.IsNullOrEmpty(searchItem.searchValue))
            {
                string sv = searchItem.searchValue.Trim();
                filtered = filtered.Where(w => w.branchname.Contains(sv) || w.createdby.Contains(sv)).ToList();
            }

            var items = filtered.Skip(searchItem.start).Take(searchItem.length).ToList();
            return Json(new { draw = searchItem.draw, recordsTotal = filtered.Count, recordsFiltered = filtered.Count, data = items });
        }
        catch
        {
            return Json(new { draw = searchItem.draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetCountStockApprovalReport([FromBody] SearchCountStockApprovalReportViewModel searchItem)
    {
        try
        {
            DateTime? approvedStart = ParseDate(searchItem.startdate);
            DateTime? approvedEnd = ParseDate(searchItem.enddate);

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
                return Json(new { draw = searchItem.draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });
            }

            return Json(new
            {
                draw = searchItem.draw,
                recordsTotal = result.data.totalrow,
                recordsFiltered = result.data.totalrow,
                data = result.data.transactiondata
            });
        }
        catch
        {
            return Json(new { draw = searchItem.draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SubmitCountStockNew([FromBody] SubmitCountStockViewModel model)
    {
        try
        {
            var result = await _countStockAPI.SubmitCountStockAsync(new SubmitCountStockCommand
            {
                countstockid = model.CountStockID,
                submittedby = base.UserProfile.username
            });
            if (!result.result)
                return Json(new { result = false, message = result.error?.error?.message ?? "ส่งข้อมูลไม่สำเร็จ" });
            return Json(new { result = true, message = "ส่งข้อมูลนับสต๊อกสำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย เกิดข้อผิดพลาด: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ApproveCountStockNew([FromBody] ApproveCountStockViewModel model)
    {
        try
        {
            if (base.UserProfile.roleid != (int)UserRole.Admin)
                return Json(new { result = false, message = "ไม่มีสิทธิ์อนุมัติ" });

            var result = await _countStockAPI.ApproveCountStockAsync(new ApproveCountStockCommand
            {
                countstockid = model.CountStockID,
                approvedby = base.UserProfile.username
            });
            if (!result.result)
                return Json(new { result = false, message = result.error?.error?.message ?? "อนุมัติไม่สำเร็จ" });
            return Json(new { result = true, message = "อนุมัติและปรับสต๊อกสำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย เกิดข้อผิดพลาด: {ex.Message}" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ExportCountStockExcel(int countstockid)
    {
        var res = await _countStockAPI.InquiryCountStockByStockIDAsync(
            new InquiryCountStockByIDQuery { countstockid = countstockid });

        if (!res.result || res.data == null)
            return NotFound("ไม่พบข้อมูลนับสต๊อก");

        var data   = res.data;
        var detail = data.detail ?? new List<InquiryCountStockByIDDetail>();

        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("นับสต๊อก");

        // Title
        ws.Cells[1, 1].Value = $"รายการนับสต๊อก — {data.branchname} — วันที่ {data.countstockdate:dd/MM/yyyy}";
        ws.Cells[1, 1, 1, 11].Merge = true;

        // Headers
        string[] headers = { "รหัสสินค้า", "ชื่อสินค้า", "ประเภทย่อย",
                              "สต๊อกระบบ", "ยอดนับได้", "รอเติม", "ชำรุด",
                              "ขายก่อนนับ", "รวมนับได้", "ขาด/เกิน", "หมายเหตุ" };
        for (int c = 0; c < headers.Length; c++)
            ws.Cells[2, c + 1].Value = headers[c];

        // Data rows
        int row = 3;
        foreach (var d in detail)
        {
            ws.Cells[row, 1].Value  = d.itemcode;
            ws.Cells[row, 2].Value  = d.itemname;
            ws.Cells[row, 3].Value  = d.subitemcode;
            ws.Cells[row, 4].Value  = d.qtyinbranchofstockday;
            ws.Cells[row, 5].Value  = d.countedqty;
            ws.Cells[row, 6].Value  = d.waitingtorestock;
            ws.Cells[row, 7].Value  = d.damaged;
            ws.Cells[row, 8].Value  = d.soldbeforecount;
            ws.Cells[row, 9].Value  = d.totalcounted;
            ws.Cells[row, 10].Value = d.difference;
            ws.Cells[row, 11].Value = d.itemremark;
            row++;
        }

        ws.Cells.AutoFitColumns();
        var bytes    = package.GetAsByteArray();
        string name  = $"CountStock_{data.branchname}_{data.countstockdate:yyyyMMdd}.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", name);
    }

    [HttpGet]
    [CustomAuthorize(RoleName.Admin)]
    public async Task<IActionResult> ExportCountStockApprovalReportExcel(string? startdate, string? enddate, int? branchid)
    {
        return RedirectToAction("ExportCountStockApprovalReportExcel", "Report", new { startdate, enddate, branchid });
    }

    [HttpPost]
    public async Task<IActionResult> SaveDraftCountStock([FromBody] List<NewCountStockEntryModel> items)
    {
        try
        {
            if (!items.Any())
                return Json(new { result = false, message = "ไม่พบรายการนับสต๊อก" });

            var invalidSubItems = items
                .Where(i => i.SubItemTypeID <= 0)
                .Select(i => !string.IsNullOrWhiteSpace(i.ItemCode) ? i.ItemCode : i.ItemName)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .Take(10)
                .ToList();
            if (invalidSubItems.Any())
            {
                return Json(new
                {
                    result = false,
                    message = $"พบรายการที่ไม่สามารถระบุประเภทย่อยได้ (SubItemTypeID=0): {string.Join(", ", invalidSubItems)} กรุณาตรวจสอบข้อมูลสินค้าในระบบก่อนบันทึก"
                });
            }

            var zeroQtyWithoutRemark = items.Where(i => i.CountedQty == 0 && string.IsNullOrEmpty(i.ItemRemark)).ToList();
            if (zeroQtyWithoutRemark.Any())
            {
                var types = string.Join(", ", zeroQtyWithoutRemark.Select(i => i.SubItemCode).Distinct());
                return Json(new { result = false, message = $"กรุณาระบุหมายเหตุสำหรับรายการที่นับได้ 0: {types}" });
            }

            var command = PrepareNewCountStockCommand(items, statusId: 0); // Draft
            var res = await _countStockAPI.CreateCountStockListV2Async(command);
            if (!res.result)
                return Json(new { result = false, message = res.error?.error?.message ?? "บันทึกแบบร่างไม่สำเร็จ กรุณาลองใหม่" });
            return Json(new { result = true, message = "บันทึกแบบร่างสำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย เกิดข้อผิดพลาด: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SubmitNewCountStock([FromBody] List<NewCountStockEntryModel> items)
    {
        try
        {
            if (!items.Any())
                return Json(new { result = false, message = "ไม่พบรายการนับสต๊อก" });

            var invalidSubItems = items
                .Where(i => i.SubItemTypeID <= 0)
                .Select(i => !string.IsNullOrWhiteSpace(i.ItemCode) ? i.ItemCode : i.ItemName)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .Take(10)
                .ToList();
            if (invalidSubItems.Any())
            {
                return Json(new
                {
                    result = false,
                    message = $"พบรายการที่ไม่สามารถระบุประเภทย่อยได้ (SubItemTypeID=0): {string.Join(", ", invalidSubItems)} กรุณาตรวจสอบข้อมูลสินค้าในระบบก่อนส่งข้อมูล"
                });
            }

            var zeroQtyWithoutRemark = items.Where(i => i.CountedQty == 0 && string.IsNullOrEmpty(i.ItemRemark)).ToList();
            if (zeroQtyWithoutRemark.Any())
            {
                var types = string.Join(", ", zeroQtyWithoutRemark.Select(i => i.SubItemCode).Distinct());
                return Json(new { result = false, message = $"กรุณาระบุหมายเหตุสำหรับรายการที่นับได้ 0: {types}" });
            }

            var command = PrepareNewCountStockCommand(items, statusId: 1); // Submitted
            var res = await _countStockAPI.CreateCountStockListV2Async(command);
            if (!res.result)
                return Json(new { result = false, message = res.error?.error?.message ?? "ส่งข้อมูลไม่สำเร็จ กรุณาลองใหม่" });
            return Json(new { result = true, message = "ส่งข้อมูลนับสต๊อกสำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"ขออภัย เกิดข้อผิดพลาด: {ex.Message}" });
        }
    }

    #endregion

    #region Private Method
    private async Task<List<SelectListItem>> PrepareSelectBranch()
    {

        BaseResponse<List<GetBranchResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();

        var dsds = resBranch.data.Remove(new GetBranchResponseDTO { branchid = 3 });
        resBranch.data = base.UserProfile.roleid == (int)EnumModel.UserRole.Sale
            ? resBranch.data.Where(w => base.UserProfile.access_branch.Select(s => s.branchid).Contains(w.branchid)).ToList()
            : resBranch.data;
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }

    private async Task<List<SelectListItem>> PrepareSelectItemType()
    {
        BaseResponse<List<GetItemTypeListResponseDTO>> resBranch = await _itemTypeAPI.GetItemTypeListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.itemtypename, Value = s.itemtypename }).ToList();
    }

    // SubItemType dropdown for comparison and new-entry pages — uses actual DB codes (e.g. CASEHONOR, GA001)
    private async Task<List<SelectListItem>> PrepareSelectSubItemType()
    {
        var res = await _subItemTypeAPI.GetSubItemTypeListAsync();
        if (!res.result || res.data == null) return new List<SelectListItem>();
        return res.data
            .Where(s => s.isactive)
            .OrderBy(s => s.subitemcode)
            .Select(s => new SelectListItem { Text = s.subitemcode, Value = s.subitemcode })
            .ToList();
    }

    private CreateCountStockCommandV1 PrepareCreateCountStockData(List<CountStockCreateModel> countStockModel)
    {
        CreateCountStockCommandV1 createCountStockCommand = new CreateCountStockCommandV1
        {
            branchid = countStockModel.FirstOrDefault().BranchID,
            countstockdate = DateTime.Now,
            createdby = base.UserProfile.username,
            remark = countStockModel.FirstOrDefault()?.Remark,
            totalcount = countStockModel.Sum(s => s.TotalCounted),
            detail = countStockModel.Select(s => new CreateCountStockDetailV1
            {
                subitemtypeid = s.SubItemTypeID > 0 ? s.SubItemTypeID : 0,
                qtyinbranchofcountstockday = s.QtyInBranchOfStockDay,
                qtyinbranch = s.StoreStock,
                countedamountqty = s.CountedQty,
                damagedqty = s.Damaged,
                salebeforecountqty = s.SoldBeforeCount,
                pendingrestockqty = s.WaitingToRestock
            }).ToList()
        };
        return createCountStockCommand;
    }

    private UpdateCountStockCommand PrepareUpdateCountStockData(List<CountStockUpdateModel> countStockModel)
    {
        var countStock = countStockModel.FirstOrDefault();
        UpdateCountStockCommand createCountStockCommand = new UpdateCountStockCommand
        {
            countstockid = countStock.CountStockID,
            branchid = countStock.BranchID,
            countstockdate = DateTime.Now,
            updatedby = base.UserProfile.username,
            remark = countStock?.Remark,
            totalcount = countStockModel.Sum(s => s.TotalCounted),
            detail = countStockModel.Select(s => new UpdateCountStockDetail
            {
                countstockdetailid = s.CountStockDetailID,
                subitemtypeid = s.SubItemTypeID > 0 ? s.SubItemTypeID : 0,
                qtyinbranchofcountstockday = s.QtyInBranchOfStockDay,
                qtyinbranch = s.StoreStock,
                countedamountqty = s.CountedQty,
                damagedqty = s.Damaged,
                salebeforecountqty = s.SoldBeforeCount,
                pendingrestockqty = s.WaitingToRestock,
            }).ToList()
        };
        return createCountStockCommand;
    }

    private CreateCountStockCommandV2 PrepareNewCountStockCommand(List<NewCountStockEntryModel> items, int statusId)
    {
        string counterRole = items.FirstOrDefault()?.CounterRole ?? "PC";
        return new CreateCountStockCommandV2
        {
            branchid = items.FirstOrDefault()!.BranchID,
            countstockdate = DateTime.Now,
            createdby = base.UserProfile.username,
            remark = items.FirstOrDefault()?.Remark,
            totalcount = items.Sum(s => s.TotalCounted),
            counterstockstatusid = statusId,
            counterrole = counterRole,
            ispartialsave = items.FirstOrDefault()?.IsPartialSave ?? false,
            detail = items.Select(s => new CreateCountStockDetailV2
            {
                itemid = s.ItemId,
                subitemtypeid = s.SubItemTypeID > 0 ? s.SubItemTypeID : 0,
                qtyinbranchofcountstockday = s.CYStockQty,
                qtyinbranch = s.CYStockQty,
                countedamountqty = s.CountedQty,
                damagedqty = s.Damaged,
                salebeforecountqty = s.SoldBeforeCount,
                pendingrestockqty = s.WaitingToRestock,
                itemremark = s.ItemRemark
            }).ToList()
        };
    }

    private static DateTime? ParseDate(string? dateStr)
    {
        if (string.IsNullOrWhiteSpace(dateStr)) return null;

        string value = dateStr.Trim();
        string[] formats =
        {
            "dd/MM/yyyy",
            "d/M/yyyy",
            "dd-MM-yyyy",
            "d-M-yyyy",
            "yyyy-MM-dd",
            "yyyy/MM/dd"
        };

        if (DateTime.TryParseExact(
                value,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsed)
            || DateTime.TryParseExact(
                value,
                formats,
                CultureInfo.GetCultureInfo("th-TH"),
                DateTimeStyles.None,
                out parsed)
            || DateTime.TryParse(value, CultureInfo.GetCultureInfo("th-TH"), DateTimeStyles.None, out parsed)
            || DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
        {
            return parsed.Date;
        }

        return null;
    }

    #endregion
}