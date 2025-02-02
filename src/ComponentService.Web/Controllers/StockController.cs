using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.CountStockAPI;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using Microsoft.AspNetCore.Mvc.Rendering;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.ExternalService.ItemTransferAPI;
using CYRetailIMS.Application.ExternalService.SubItemTypeAPI;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.AreaSale, RoleName.Stock, RoleName.Sale)]
public class StockController : BaseController
{
    private readonly ICountStockAPI _countStockAPI;
    private readonly IBranchAPI _branchAPI;
    private readonly IItemInBranchAPI _itemInBranchAPI;
    private readonly ISubItemTypeAPI _subItemTypeAPI;

    public StockController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        ICountStockAPI countStockAPI,
        IBranchAPI branchAPI,
        IItemInBranchAPI itemInBranchAPI,
        ISubItemTypeAPI subItemTypeAPI) : base(httpClientRequest, mapper, log)
    {
        _countStockAPI = countStockAPI;
        _branchAPI = branchAPI;
        _itemInBranchAPI = itemInBranchAPI;
        _subItemTypeAPI = subItemTypeAPI;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public IActionResult CountStock()
    {
        return View();
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


    #endregion
}
