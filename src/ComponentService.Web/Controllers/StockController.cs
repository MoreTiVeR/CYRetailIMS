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
        var items = new List<CountStockItemViewModel>
        {
            new CountStockItemViewModel
            {
                ItemTypeCode = "Case",
                SubItemCode = "CASEHONOR",
                ItemId = 101,
                StoreStock = 50,
                CountedQty = 48,
                WaitingToRestock = 10,
                Damaged = 2,
                SoldBeforeCount = 5,
                TotalCounted = 55,
                Difference = -2
            },
            new CountStockItemViewModel
            {
                ItemTypeCode = "Case",
                SubItemCode = "CASEHUAWEI",
                ItemId = 102,
                StoreStock = 30,
                CountedQty = 30,
                WaitingToRestock = 5,
                Damaged = 1,
                SoldBeforeCount = 2,
                TotalCounted = 37,
                Difference = 0
            },
            new CountStockItemViewModel
            {
                ItemTypeCode = "Film",
                SubItemCode = "GA001",
                ItemId = 103,
                StoreStock = 100,
                CountedQty = 95,
                WaitingToRestock = 20,
                Damaged = 3,
                SoldBeforeCount = 10,
                TotalCounted = 125,
                Difference = -5
            },
            new CountStockItemViewModel
            {
                ItemTypeCode = "Equipment",
                SubItemCode = "GD015",
                ItemId = 104,
                StoreStock = 75,
                CountedQty = 80,
                WaitingToRestock = 15,
                Damaged = 0,
                SoldBeforeCount = 8,
                TotalCounted = 103,
                Difference = 5
            },
            new CountStockItemViewModel
            {
                ItemTypeCode = "Equipment",
                SubItemCode = "GB021",
                ItemId = 105,
                StoreStock = 60,
                CountedQty = 58,
                WaitingToRestock = 12,
                Damaged = 1,
                SoldBeforeCount = 4,
                TotalCounted = 74,
                Difference = -2
            },
            new CountStockItemViewModel
            {
                ItemTypeCode = "Equipment",
                SubItemCode = "GD016",
                ItemId = 101,
                StoreStock = 50,
                CountedQty = 48,
                WaitingToRestock = 10,
                Damaged = 2,
                SoldBeforeCount = 5,
                TotalCounted = 55,
                Difference = -2
            }
        };

        ViewBag.ItemTypeList = await PrepareSelectItemType();
        ViewBag.BranchList = await PrepareSelectBranch();
        return View(items);
    }


    #region Private Method
    private async Task<List<SelectListItem>> PrepareSelectBranch()
    {

        BaseResponse<List<GetBranchResponseDTO>> resBranch = await _branchAPI.GetBranchListAsync();
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
    #endregion

    #region Http Method

    [HttpPost]
    public async Task<IActionResult> GetCountStocks([FromBody] SearchCountStockViewModel searchItem)
    {
        BaseResponse<List<InquiryCountStockResponseDTO>> countstockData = new BaseResponse<List<InquiryCountStockResponseDTO>> { data = new List<InquiryCountStockResponseDTO>() };
        try
        {
            #region Prepare Search Start & End Date
            DateTime? transferSrtartDate = null;
            DateTime? transferEndDate = null;
            int? branchID = null;
            if (!string.IsNullOrEmpty(searchItem.startdate))
            {
                string[] sTransferDate = searchItem.startdate.Split("-");
                if (sTransferDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                transferSrtartDate = new DateTime(sTransferDate[2].ToInt32(), sTransferDate[1].ToInt32(), sTransferDate[0].ToInt32());
            }

            if (!string.IsNullOrEmpty(searchItem.enddate))
            {
                string[] sTransferEndDate = searchItem.enddate.Split("-");
                if (sTransferEndDate.Count() != 3)
                {
                    throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
                }
                transferEndDate = new DateTime(sTransferEndDate[2].ToInt32(), sTransferEndDate[1].ToInt32(), sTransferEndDate[0].ToInt32());
            }

            //เช็ควันที่สิ้นสุดน้อยกว่า วันเริ่มต้น
            if ((transferSrtartDate.HasValue && transferEndDate.HasValue)
                && DateTime.Compare(transferSrtartDate.Value, transferEndDate.Value) == 1)
            {
                throw new Exception("รุปแบบวันที่ในการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
            }
            #endregion

            //Set branchid & transfer status
            branchID = searchItem.branchid == 999 ? null : searchItem.branchid;
            if (base.UserProfile.roleid == (int)UserRole.Admin || base.UserProfile.roleid == (int)UserRole.Stock)
            {
                countstockData = await _countStockAPI.GetCountStockListAsync(new InquiryCountStocksQuery { });
            }
            else
            {
                countstockData = await _countStockAPI.GetCountStockListAsync(new InquiryCountStocksQuery { });
            }

            if (!countstockData.result)
            {
                return Json(new { data = new List<InquiryCountStockResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
            }

            #region Search Filter
            //if (!string.IsNullOrEmpty(searchItem.searchValue))
            //{
            //    string searchValue = searchItem.searchValue.Replace("\t", "").Replace("\n", "");
            //    countstockData.data = countstockData.data.Where(w => w.itemname.Contains(searchValue)
            //    || w.destinationname.Contains(searchValue)
            //    || w.transferstatusname_th.Contains(searchValue)
            //    || w.transfertypename.Contains(searchValue)
            //    || w.createdby.Contains(searchValue)).ToList();
            //}
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
    public IActionResult SaveData([FromBody] CountStockModel model)
    {
        if (ModelState.IsValid)
        {
            // Save your model to the database
            // Example: _context.YourEntities.Add(model);
            // _context.SaveChanges();

            return Json(new { success = true });
        }
        return Json(new { success = false, errors = ModelState });
    }

    // POST: Save updated stock counts
    [HttpPost]
    public IActionResult Save([FromBody] List<CountStockUpdateModel> updatedItems)
    {
        // Perform the save operation (e.g., update the database)
        foreach (var item in updatedItems)
        {
            // Update logic here
            // Example: UpdateStock(item.ItemId, item.NewQty);
        }

        return Ok(new { message = "Stock counts updated successfully!" });
    }
    #endregion
}

public class CountStockModel
{
    public List<CountStockDetail> products { get; set; }
}

public class CountStockDetail
{
    public string ProductCode { get; set; }
    public int Stock { get; set; }
    public int Count { get; set; }
}

// ViewModel for displaying data
public class CountStockItemViewModel
{
    public string ItemTypeCode { get; set; }
    public string SubItemCode { get; set; }
    public int ItemId { get; set; }
    public int StoreStock { get; set; }
    public int CountedQty { get; set; }
    public int WaitingToRestock { get; set; }
    public int Damaged { get; set; }
    public int SoldBeforeCount { get; set; }
    public int TotalCounted { get; set; }
    public int Difference { get; set; }
}

// Model for receiving updated data
public class CountStockUpdateModel
{
    public string ItemTypeCode { get; set; }
    public string SubItemCode { get; set; }
    public int ItemId { get; set; }
    public int StoreStock { get; set; }
    public int CountedQty { get; set; }
    public int WaitingToRestock { get; set; }
    public int Damaged { get; set; }
    public int SoldBeforeCount { get; set; }
    public int TotalCounted { get; set; }
    public int Difference { get; set; }
}