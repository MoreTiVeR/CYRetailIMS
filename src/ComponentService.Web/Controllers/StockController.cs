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
using CYRetailIMS.Infrastructure.Database;
using CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.UpdateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.DeleteCountStock.v1;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.AreaSale, RoleName.Stock, RoleName.Sale)]
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
            CreateCountStockCommand countStockCommand = PrepareCreateCountStockData(updatedItems);
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

    private CreateCountStockCommand PrepareCreateCountStockData(List<CountStockCreateModel> countStockModel)
    {
        CreateCountStockCommand createCountStockCommand = new CreateCountStockCommand
        {
            branchid = countStockModel.FirstOrDefault().BranchID,
            countstockdate = DateTime.Now,
            createdby = base.UserProfile.username,
            remark = countStockModel.FirstOrDefault()?.Remark,
            totalcount = countStockModel.Sum(s => s.TotalCounted),
            detail = countStockModel.Select(s => new CreateCountStockDetail
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
    #endregion
}