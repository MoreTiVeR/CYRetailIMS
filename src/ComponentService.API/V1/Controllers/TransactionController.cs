using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;
using CYRetailIMS.Application.Services.TransactionService.Commands.UpdateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v2;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByCriteria.v1;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByTransactionID.v1;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/transaction")]
public class TransactionController : BaseApiController
{
    public TransactionController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTransactionAsync(CreateTransactionCommand createTransactionCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createTransactionCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateItemAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/delete")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTransactionByTransactionIDAsync(DeleteTransactionCommand deleteTransactionCommand)
    {
        BaseResponse<CommandResponse> res = await Mediator.Send(deleteTransactionCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]DeleteTransactionByTransactionIDAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/update")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDataTransactionAsync(UpdateTransactionCommand updateTransactionCommand)
    {
        BaseResponse<CommandResponse> res = await Mediator.Send(updateTransactionCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateDataTransactionAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/transactionbybranchid/{branchid:int}")]
    [ProducesResponseType(typeof(List<GetTransactionByBranchIDResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTransactionByBrachIDAsync(int branchid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetTransactionByBranchIDResponseDTO>> res = await Mediator.Send(new GetTransactionByBranchIDQuery { branchid = branchid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateTransactionByBrachIDAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v2/transactionbybranch")]
    [ProducesResponseType(typeof(GetTransactionByBranchIDV2ReseponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTransactionByBrachIDV2Async(GetTransactionByBranchIDV2Query reqGetTransactionByBranch)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetTransactionByBranchIDV2ReseponseDTO> res = await Mediator.Send(reqGetTransactionByBranch);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateTransactionByBrachIDV2Async Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/transactionbyid/{transactionid:int}")]
    [ProducesResponseType(typeof(GetTransactionByBranchIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactionByTransactionIDAsync(int transactionid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetTransactionByBranchIDResponseDTO> res = await Mediator.Send(new GetTransactionByTransactionIDQuery { transactionid = transactionid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetTransactionByTransactionIDAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/transaction")]
    [ProducesResponseType(typeof(GetTransactionByCriteriaResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactionByCriteriaAsync(GetTransactionByCriteriaQuery getTransactionByCriteria)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetTransactionByCriteriaResponseDTO > res = await Mediator.Send(getTransactionByCriteria);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetTransactionByCriteriaAsync Success");
        return Ok(res.data);
    }
}
