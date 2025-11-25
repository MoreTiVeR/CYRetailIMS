using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;
using CYRetailIMS.Application.Services.TransactionService.Commands.UpdateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.Application.Common.Models.EnumModel;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByCriteria.v1;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.AreaSale)]
public class TransactionsController : BaseController
{
    private readonly ITransactionAPI _transactionAPI;
    public TransactionsController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        ITransactionAPI transactionAPI) : base(httpClientRequest, mapper, log)
    {
        _transactionAPI = transactionAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Edit(int tranid)
    {
        BaseResponse<GetTransactionByCriteriaResponseDTO> resTransaction = await _transactionAPI.GetTransactionByCriteriaAsync(new GetTransactionByCriteriaQuery
        {
            transactionid = tranid,
            branchid = UserProfile.access_branch.FirstOrDefault().branchid
        });
        if (!resTransaction.result)
        {
            throw new Exception("ไม่พบข้อมูล");
        }

        EditTransactionViewModel tranViewModel = _mapper.Map<EditTransactionViewModel>(resTransaction.data);
        int seq = 1;
        tranViewModel.Detail = tranViewModel.Detail.Select(s =>
        {
            s.Seq = seq;
            s.TransactionID = tranViewModel.TransactionID;
            seq++;
            return s;
        }).ToList();
        return View(tranViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTransaction([FromBody] EditTransactionViewModel editTranObj)
    {
        try
        {
            if (!base.UserProfile.access_branch.Select(s => s.branchid).Contains(editTranObj.BranchID))
            {
                return Json(new JsonViewModel { result = false, message = "ขออภัย, คุณไม่มีสิทธิ์ในการทำรายการ" });
            }
            UpdateTransactionCommand updateTransactionCommand = PrepareUpdateTransactionData(editTranObj);
            BaseResponse<CommandResponse> resUpdateItem = await _transactionAPI.UpdateTransactionAsync(updateTransactionCommand);
            if (resUpdateItem.result)
            {
                return Json(new JsonViewModel { result = resUpdateItem.result, message = "ปรับปรุงข้อมูลสำเร็จ" });
            }
            return Json(new JsonViewModel { result = resUpdateItem.result, message = resUpdateItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTransaction([FromBody] DeleteTransactionViewModel deleteTranObj)
    {
        try
        {
            BaseResponse<CommandResponse> resDeleteTran = await _transactionAPI.DeleteTransactionByIDAsync(new DeleteTransactionCommand
            {
                transactionid = deleteTranObj.transactionid,
                deletedby = base.UserProfile.username
            });
            //if (resDeleteTran.result)
            //{
            //    return Json(new JsonViewModel { result = resDeleteTran.result, message = resDeleteTran.result ? resDeleteTran.message : resDeleteTran.error.error.message });
            //}
            //return Json(new JsonViewModel { result = resDeleteTran.result, message = resDeleteTran.error.error.message });
            return Json(new JsonViewModel 
            { 
                result = resDeleteTran.data.result, 
                message = resDeleteTran.data.result ? "ลบข้อมูลสำเร็จ" : resDeleteTran.data.error.message
            });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    #region Private
    private UpdateTransactionCommand PrepareUpdateTransactionData(EditTransactionViewModel editTranObj)
    {
        return new UpdateTransactionCommand
        {
            transactionid = editTranObj.TransactionID,
            updatedby = base.UserProfile.username,
            transactiondate = editTranObj.TransactionDate.ToDate()
        };
    }
    #endregion
}
