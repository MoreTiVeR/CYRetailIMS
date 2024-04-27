using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

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
        return View();
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
            if (resDeleteTran.result)
            {
                return Json(new JsonViewModel { result = resDeleteTran.result, message = resDeleteTran.message });
            }
            return Json(new JsonViewModel { result = resDeleteTran.result, message = resDeleteTran.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }
}
