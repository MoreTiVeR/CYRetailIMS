using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.ExternalService.ReceiptTempAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByCriteria.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin)]
public class ReceiptController : BaseController
{
    private readonly IReceiptTempAPI _receiptTempAPI;
    private readonly IBranchAPI _branchAPI;
    public ReceiptController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IReceiptTempAPI receiptTempAPI,
        IBranchAPI branchAPI) : base(httpClientRequest, mapper, log)
    {
        _receiptTempAPI = receiptTempAPI;
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

    public async Task<IActionResult> Edit(int receivetempid)
    {
        BaseResponse<GetReceiveTempResponseDTO> rTemplate = await _receiptTempAPI.GetReceiptTemplateByIDAsync(new GetReceiveTempByIDQuery
        {
            tempreceiveid = receivetempid
        });
        EditReceiptTemplateViewModel vModel = MappingEditModel(rTemplate.data);
        return View(vModel);
    }

    [HttpPost]
    public async Task<IActionResult> SearchReceipt([FromBody] SearchReceiptTemplateViewModel searchItem)
    {
        List<GetReceiveTempResponseDTO> resReceiptList = new List<GetReceiveTempResponseDTO>();
        try
        {
            int? branchID = null;
            branchID = searchItem.branchid == 999 || searchItem.branchid == 0 ? null : searchItem.branchid;
            BaseResponse<GetReceiveTempByCriteriaResponseDTO> resReport = await _receiptTempAPI.GetReceiptTemplateByCriteriaAsync(new GetReceiveTempByCriteriaQuery
            {
                branchid = branchID,
                startrow = searchItem.start,
                pagesize = searchItem.length,
                searchvalue = searchItem.searchValue.Replace("\t", "").Replace("\n", ""),
                isexportalldata = searchItem.isexportalldata,
            });

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

            var totalItems = resReport.data.totalrow; // Get total item count for pagination

            // Filter based on searchValue if necessary
            var query = resReport.data.receipttemplates;

            // Prepare response for DataTables
            return Json(new
            {
                draw = searchItem.draw, // Echo the draw parameter
                recordsTotal = totalItems, // Total records before filtering
                recordsFiltered = totalItems, // Total records after applying filtering
                data = resReport.data.receipttemplates // The actual data to be displayed
            });
        }
        catch
        {
            return Json(new { data = new List<GetReceiveTempResponseDTO>(), recordsTotal = 0, recordsFiltered = 0 });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateReceiptTemplate([FromBody] CreateReceiptTemplateViewModel createReceipt)
    {
        try
        {
            CreateReceiveTemplateCommand createReceiveCmd = PrepareCreateReceiptCommand(createReceipt);
            BaseResponse<CommandResponse> resCreate = await _receiptTempAPI.CreateBranchAsync(createReceiveCmd);
            return Json(new JsonViewModel { result = resCreate.result, message = resCreate.result ? resCreate.message : resCreate.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditReceiptTemplate([FromBody] EditReceiptTemplateViewModel editReceipt)
    {
        try
        {
            UpdateReceiveTemplateCommand updateReceiptTempCommand = PrepareUpdateReceiptCommand(editReceipt);
            BaseResponse<CommandResponse> resUpdateReceiptTemp = await _receiptTempAPI.UpdateBranchAsync(updateReceiptTempCommand);
            return Json(new JsonViewModel { result = resUpdateReceiptTemp.result, message = resUpdateReceiptTemp.result ? resUpdateReceiptTemp.message : resUpdateReceiptTemp.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }


    [HttpPost]
    public async Task<IActionResult> DeleteReceiptByID([FromBody] DeleteReceiptTemplateViewModel deleteReceiptTemp)
    {
        try
        {
            DeleteReceiveTemplateCommand delItemCommand = new DeleteReceiveTemplateCommand { receivetemplateid = deleteReceiptTemp.receipttempid, updatedby = base.UserProfile.username };
            BaseResponse<CommandResponse> resDelete = await _receiptTempAPI.DeleteBranchAsync(delItemCommand);
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

    private EditReceiptTemplateViewModel MappingEditModel(GetReceiveTempResponseDTO getReceiveTempResponseDTO)
    {
        return new EditReceiptTemplateViewModel
        {
            receivetempid = getReceiveTempResponseDTO.receivetempid,
            branchid = getReceiveTempResponseDTO.branchid,
            //branchcode = getReceiveTempResponseDTO.branchcode,
            //branchname = getReceiveTempResponseDTO.branchname,
            shopheadernametext = getReceiveTempResponseDTO.shopheadernametext,
            shopheaderaddresstext = getReceiveTempResponseDTO.shopheaderaddresstext,
            shopfootertext = getReceiveTempResponseDTO.shopfootertext,
            additionalfootertext = getReceiveTempResponseDTO.additionalfootertext,
            telephoneno = getReceiveTempResponseDTO.telephoneno,
            printername = getReceiveTempResponseDTO.printername,
            updatedby = base.UserProfile.username,
            isactive = getReceiveTempResponseDTO.isactive.ToString()
        };
    }

    private CreateReceiveTemplateCommand PrepareCreateReceiptCommand(CreateReceiptTemplateViewModel createReceipt)
    {
        return new CreateReceiveTemplateCommand
        {
            branchid = createReceipt.branchid.ToInt32(),
            shopheadernametext = createReceipt.shopheadernametext,
            shopheaderaddresstext = createReceipt.shopheaderaddresstext,
            shopfootertext = createReceipt.shopfootertext,
            additionalfootertext = createReceipt.additionalfootertext,
            telephoneno = createReceipt.telephoneno,
            printername = createReceipt.printername,
            createdby = base.UserProfile.username
        };
    }

    private UpdateReceiveTemplateCommand PrepareUpdateReceiptCommand(EditReceiptTemplateViewModel editReceipt)
    {
        return new UpdateReceiveTemplateCommand
        {
            receivetemplateid = editReceipt.receivetempid,
            branchid = editReceipt.branchid,
            shopheadernametext = editReceipt.shopheadernametext,
            shopheaderaddresstext = editReceipt.shopheaderaddresstext,
            shopfootertext = editReceipt.shopfootertext,
            additionalfootertext = editReceipt.additionalfootertext,
            telephoneno = editReceipt.telephoneno,
            printername = editReceipt.printername,
            updatedby = base.UserProfile.username,
            isactive = editReceipt.isactive.ToBoolFromIntString()
        };
    }

    #endregion
}
