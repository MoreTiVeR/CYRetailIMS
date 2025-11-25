using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.EndOfDaySummaryAPI;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.CreateEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.DeleteEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.UpdateEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByID.v1;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryList.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class EndOfDaySummaryController : BaseController
{
    private readonly IEndOfDaySummaryAPI _endOfDaySummaryAPI;
    private readonly ITransactionAPI _transactionAPI;

    public EndOfDaySummaryController(IHttpClientRequest httpClientRequest, 
        IMapper mapper, ILog4NetLogger log,
        IEndOfDaySummaryAPI endOfDaySummaryAPI,
        ITransactionAPI transactionAPI) : base(httpClientRequest, mapper, log)
    {
        _endOfDaySummaryAPI = endOfDaySummaryAPI;
        _transactionAPI = transactionAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Create()
    {
        // prepare default model for the Create view
        var model = new EndOfDaySummaryViewModel()
        {
            EndOfDayId = null,
            SummaryDate = DateTime.Now.ToString("dd-MM-yyyy"),
            TotalCash = 0m,
            TotalTransfer = 0m,
            GrandTotal = 0m,
            //DepositedCash = 0m,
            //CustomerTransfer = 0m,
            //SubstituteWage = 0m,
            //Fee = 0m,
            //OtherExpense = 0m,
            //OtherExpenseNote = string.Empty,
            //FinalTotal = 0m,
            IsActive = true,
            CurrentUserName = base.UserProfile.username
        };

        BaseResponse<GetTransactionByBranchIDV2ReseponseDTO> curTransaciton = await _transactionAPI.GetTransactionByBranchIDV2Async(new GetTransactionByBranchIDV2Query
        {
            branchid = base.UserProfile.access_branch.FirstOrDefault().branchid,
            transaction_startdate = DateTime.Now,
            transaction_enddate = DateTime.Now,
            startrow = 0,
            pagesize = 10
        });
        if (!curTransaciton.result)
        {
            TempData["ErrorMessage"] = "ไม่สามารถสรุปยอดขายได้, เนื่องจากไม่พบข้อมูลขาย ณ วันปัจจุบัน";
            return View("Create", model);
        }

        model.TotalCash = curTransaciton.data.totalcash;
        model.TotalTransfer = curTransaciton.data.totaltransfer;
        if((curTransaciton.data.totalcash + curTransaciton.data.totaltransfer + curTransaciton.data.totaldepositfee) != curTransaciton.data.totalamount)
        {
            TempData["ErrorMessage"] = "ไม่สามารถสรุปยอดขายได้, เนื่องจากข้อมูลรายการขายไม่ถูกต้อง";
            return View("Create", model);
        }
        model.GrandTotal = curTransaciton.data.totalamount;

        return View(model);
    }

    public async Task<IActionResult> Edit(int tranid)
    {
        BaseResponse<GetEndOfDaySummaryByCriteriaDetail> curEodSummaryData = await _endOfDaySummaryAPI.SearchEndOfDaySummaryByIDAsync(new GetEndOfDaySummaryByIDQuery
        {
            eodid = tranid
        });

        if (!curEodSummaryData.result)
        {
            TempData["ErrorMessage"] = "ไม่สามารถสรุปยอดขายได้, เนื่องจากไม่พบข้อมูลขาย ณ วันปัจจุบัน";
            return View("Index");
        }

        if(curEodSummaryData.data.branchid != base.UserProfile.access_branch.FirstOrDefault().branchid)
        {
            TempData["ErrorMessage"] = "ไม่สามารถดำเนินการได้, เนื่องจากข้อมูลรายการและสาขาไม่ถูกต้อง";
            return View("Index");
        }

        BaseResponse<GetTransactionByBranchIDV2ReseponseDTO> curTransaciton = await _transactionAPI.GetTransactionByBranchIDV2Async(new GetTransactionByBranchIDV2Query
        {
            branchid = curEodSummaryData.data.branchid,
            transaction_startdate = curEodSummaryData.data.summarydate,
            transaction_enddate = curEodSummaryData.data.summarydate,
            startrow = 0,
            pagesize = 10
        });
        if (!curTransaciton.result)
        {
            TempData["ErrorMessage"] = "ไม่สามารถสรุปยอดขายได้, เนื่องจากไม่พบข้อมูลขายวันที่สรุปรายการ";
            return View("Index");
        }

        #region Binding View Data
        var model = new EndOfDaySummaryViewModel()
        {
            EndOfDayId = curEodSummaryData.data.endofdayid,
            SummaryDate = curEodSummaryData.data.summarydate.ToDateString(),
            TotalCash = curTransaciton.data.totalamount, // transaction
            TotalTransfer = curTransaciton.data.totaltransfer, // transaction
            GrandTotal = curTransaciton.data.totalamount, // transaction
            DepositedCash = curEodSummaryData.data.depositedcash,
            CustomerTransfer = curEodSummaryData.data.totaltransfer,
            SubstituteWage = curEodSummaryData.data.substitutewage,
            Fee = curEodSummaryData.data.fee,
            OtherExpense = curEodSummaryData.data.otherexpense,
            OtherExpenseNote = curEodSummaryData.data.otherexpensenote,
            FinalTotal = curEodSummaryData.data.finaltotal,
            IsActive = curEodSummaryData.data.isactive,
            //CurrentUserName = base.UserProfile.username
        };
        #endregion
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> SearchData([FromBody] SearchEndOfDaySummaryViewModel searchItem)
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

            branchID = base.UserProfile.roleid == (int)EnumModel.UserRole.Admin ? null : base.UserProfile.access_branch.FirstOrDefault().branchid;
            BaseResponse<GetEndOfDaySummaryByCriteriaResponseDTO> resReport = await _endOfDaySummaryAPI.GetEndOfDaySummaryByCriteriaAsync(new GetEndOfDaySummaryByCriteriaQuery
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
                return Json(new { data = new List<GetEndOfDaySummaryByCriteriaDetail>(), recordsTotal = 0, recordsFiltered = 0 });
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
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return Json(new { data = new List<GetEndOfDaySummaryByCriteriaDetail>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEodSummary(EndOfDaySummaryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Create", model);
        }

        if (!EodSummaryDataValidation(model))
        {
            TempData["ErrorMessage"] = "จำนวนเงินรวมสุทธิไม่ถูกต้อง, กรุณาตรวจสอบใหม่อีกครั้ง";
            return View("Create", model);
        }

        // parse user info from session
        string username = string.Empty;
        int branchId = 0;
        DateTime summaryDate = DateTime.Now;
        try
        {
            //Get branchid and username from session
            branchId = base.UserProfile.access_branch.FirstOrDefault().branchid;

            // parse date string (expect dd-MM-yyyy)
            if (!string.IsNullOrEmpty(model.SummaryDate))
            {
                if (!DateTime.TryParseExact(model.SummaryDate, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out summaryDate))
                {
                    // try other formats
                    DateTime.TryParse(model.SummaryDate, out summaryDate);
                }
            }

            var cmd = new CreateEndOfDaySummaryCommand
            {
                summarydate = summaryDate,
                branchid = branchId,
                totalcash = model.TotalCash,
                depositedcash = model.DepositedCash,
                totaltransfer = model.TotalTransfer,
                customertransfer = model.CustomerTransfer,
                grandtotal = model.GrandTotal,
                substitutewage = model.SubstituteWage,
                fee = model.Fee,
                otherexpense = model.OtherExpense,
                otherexpensenote = model.OtherExpenseNote,
                finaltotal = model.FinalTotal,
                isactive = model.IsActive,
                createdby = username
            };

            var res = await _endOfDaySummaryAPI.CreateEndOfDaySummaryAsync(cmd);
            if (res == null || !res.result)
            {
                TempData["ErrorMessage"] = res?.error?.error?.message;
                return View("Create", model);
            }

            TempData["SuccessMessage"] = "บันทึกข้อมูลสำเร็จ";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View("Create", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEodSummary(EndOfDaySummaryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        if (!EodSummaryDataValidation(model))
        {
            TempData["ErrorMessage"] = "จำนวนเงินรวมสุทธิไม่ถูกต้อง, กรุณาตรวจสอบใหม่อีกครั้ง";
            return View("Create", model);
        }

        // parse user info from session
        string username = string.Empty;
        int branchId = 0;
        DateTime summaryDate = DateTime.Now;
        try
        {
            //Get branchid and username from session
            branchId = base.UserProfile.access_branch.FirstOrDefault().branchid;

            // parse date string (expect dd-MM-yyyy)
            if (!string.IsNullOrEmpty(model.SummaryDate))
            {
                if (!DateTime.TryParseExact(model.SummaryDate, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out summaryDate))
                {
                    // try other formats
                    DateTime.TryParse(model.SummaryDate, out summaryDate);
                }
            }

            if (!model.EndOfDayId.HasValue)
            {
                TempData["ErrorMessage"] = "ไม่สามารถดำเนินการได้, เนื่องจากข้อมูลรายการและสาขาไม่ถูกต้อง";
                return View("Index");
            }

            var cmd = new UpdateEndOfDaySummaryCommand
            {
                endofdayid = model.EndOfDayId.Value,
                summarydate = summaryDate,
                totalcash = model.TotalCash,
                depositedcash = model.DepositedCash,
                totaltransfer = model.TotalTransfer,
                customertransfer = model.CustomerTransfer,
                grandtotal = model.GrandTotal,
                substitutewage = model.SubstituteWage,
                fee = model.Fee,
                otherexpense = model.OtherExpense,
                otherexpensenote = model.OtherExpenseNote,
                finaltotal = model.FinalTotal,
                isactive = model.IsActive,
                updatedby = username
            };

            var res = await _endOfDaySummaryAPI.UpdateEndOfDaySummaryAsync(cmd);
            if (res == null || !res.result)
            {
                TempData["ErrorMessage"] = res?.error?.error?.message;
                return View("Edit", model);
            }

            TempData["SuccessMessage"] = "บันทึกข้อมูลสำเร็จ";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return View("Edit", model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEodSummary([FromBody] DeleteEndOfDaySummaryViewModel request)
    {
        try
        {
            var cmd = new DeleteEndOfDaySummaryCommand
            {
                eodid = request.eodid,
                isactive = false,
                updatedby = base.UserProfile?.username ?? string.Empty
            };

            var resDelete = await _endOfDaySummaryAPI.DeleteEndOfDaySummaryAsync(cmd);
            if (resDelete == null || !resDelete.result)
            {
                return Json(new { result = false, message = "ไม่สามารถลบข้อมูลได้" });
            }

            return Json(new { result = true, message = "ลบข้อมูลสำเร็จ" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Return transaction totals for a given summary date (used by Create view when date changes)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> GetTransactionSummary([FromBody] TransactionSummaryRequest request)
    {
        try
        {
            DateTime summaryDate = DateTime.Now;
            if (!string.IsNullOrEmpty(request?.date))
            {
                // try parse common formats dd-MM-yyyy or dd/MM/yyyy
                if (!DateTime.TryParseExact(request.date, new[] { "dd-MM-yyyy", "dd/MM/yyyy" }, CultureInfo.CurrentCulture, DateTimeStyles.None, out summaryDate))
                {
                    DateTime.TryParse(request.date, out summaryDate);
                }
            }

            int branchId = base.UserProfile.access_branch.FirstOrDefault()?.branchid ?? 0;

            var res = await _transactionAPI.GetTransactionByBranchIDV2Async(new GetTransactionByBranchIDV2Query
            {
                branchid = branchId,
                transaction_startdate = summaryDate,
                transaction_enddate = summaryDate,
                startrow = 0,
                pagesize = 10
            });

            if (res == null || !res.result)
            {
                //TempData["ErrorMessage"] = "ไม่พบข้อมูลขายในวันที่ระบุ, กรุณาลองใหม่อีกครั้ง";
                //return View("Create");
                return Json(new
                {
                    result = false,
                    message = "ไม่พบข้อมูลยอดขายสำหรับวันที่เลือก, กรุณาเลือกวันสรุกยอดใหม่อีกครั้ง",
                    data = new
                    {
                        totalcash = 0,
                        totaltransfer = 0,
                        totalamount = 0,
                        totaldepositfee = 0
                    }
                });
            }

            // return totals
            return Json(new
            {
                result = true,
                data = new
                {
                    totalcash = res.data.totalcash,
                    totaltransfer = res.data.totaltransfer,
                    totalamount = res.data.totalamount,
                    totaldepositfee = res.data.totaldepositfee
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = ex.Message });
        }
    }

    public class TransactionSummaryRequest
    {
        public string? date { get; set; }
    }

    private bool EodSummaryDataValidation(EndOfDaySummaryViewModel requestObj)
    {
        try
        {
            if(requestObj.GrandTotal == 0)
            {
                return false;
            }
            decimal substituteWage = requestObj.SubstituteWage.HasValue ? requestObj.SubstituteWage.Value : 0;
            decimal otherExpense = requestObj.OtherExpense.HasValue ? requestObj.OtherExpense.Value : 0;
            decimal fee = requestObj.Fee.HasValue ? requestObj.Fee.Value : 0;
            decimal filnalTotal = requestObj.DepositedCash + requestObj.CustomerTransfer + substituteWage + fee + otherExpense;
            if(filnalTotal == 0)
            {
                return false;
            }
            return requestObj.GrandTotal == filnalTotal;
        }
        catch
        {
            return false;
        }
    }
}

