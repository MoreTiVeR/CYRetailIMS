using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ReceiveTempAPI;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class ReceiptController : BaseController
{
    private readonly IReceiveTempAPI _receiveTempAPI;
    private readonly IBranchAPI _branchAPI;
    public ReceiptController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IReceiveTempAPI receiveTempAPI,
        IBranchAPI branchAPI) : base(httpClientRequest, mapper, log)
    {
        _receiveTempAPI = receiveTempAPI;
        _branchAPI = branchAPI;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.BranchList = await PrepareSelectBranch();
        return View();
    }

    public IActionResult Edit(int receivetempid)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SearchReceipt([FromBody] SearchReceiptTemplateViewModel searchItem)
    {
        List<GetReceiveTempResponseDTO> resReceiptList = new List<GetReceiveTempResponseDTO>();
        try
        {
            int? branchID = null;
            branchID = searchItem.branchid == 999 || searchItem.branchid == 0 ? null : searchItem.branchid;
            BaseResponse<List<GetReceiveTempResponseDTO>> resReport = await _receiveTempAPI.GetReceiveTemplatehListAsync();

            if (!resReport.result)
            {
                return Json(new { data = new List<GetReceiveTempResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
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

            ////var totalRows = resReport.data.totalrow;
            //var totalItems = resReport.data.totalrow; // Get total item count for pagination

            //// Filter based on searchValue if necessary
            //var query = resReport.data.transactiondata;

            //// Calculate paginated data
            ////var items = searchItem.isexportalldata ? query : query.Skip(searchItem.start).Take(searchItem.length).ToList();

            //// Prepare response for DataTables
            //return Json(new
            //{
            //    draw = searchItem.draw, // Echo the draw parameter
            //    recordsTotal = totalItems, // Total records before filtering
            //    recordsFiltered = totalItems, // Total records after applying filtering
            //    data = resReport.data.transactiondata // The actual data to be displayed
            //});

            resReceiptList = resReport.data;
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = 100, // Total records before filtering
                recordsFiltered = 10, // Total records after applying filtering
                data = resReceiptList
            });
        }
        catch
        {
            return Json(new { data = new List<GetReceiveTempResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }


    [HttpPost]
    public async Task<IActionResult> DeleteReceiptByID([FromBody] DeleteReceiptTemplateViewModel deleteReceiptTemp)
    {
        try
        {
            DeleteReceiveTemplateCommand delItemCommand = new DeleteReceiveTemplateCommand { receivetemplateid = deleteReceiptTemp.receipttempid, updatedby = base.UserProfile.username };
            BaseResponse<CommandResponse> resDelete = await _receiveTempAPI.DeleteBranchAsync(delItemCommand);
            if (resDelete.result)
            {
                return Json(new JsonViewModel { result = resDelete.result, message = resDelete.message });
            }
            return Json(new JsonViewModel { result = resDelete.result, message = resDelete.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    #region Private
    private async Task<List<SelectListItem>> PrepareSelectBranch()
    {
        var resBranch = await _branchAPI.GetBranchListAsync();
        return resBranch.data.Select(s => new SelectListItem { Text = s.branchname, Value = s.branchid.ToString() }).ToList();
    }
    #endregion
}
