using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactionByBranchID.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactionByID.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactions.v1;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;


[Route("api/v{version:apiVersion}/adjustitem")]
[ApiController]
public class AdjustItemController : BaseApiController
{
    public AdjustItemController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AdjustItemAsync(CreateAdjustItemCommand createAdjustItemCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createAdjustItemCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]AdjustItemAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/update")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAdjustItemAsync(UpdateAdjustItemCommand updateAdjustItemCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(updateAdjustItemCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateAdjustItemAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/adjusttransactions")]
    [ProducesResponseType(typeof(List<GetAdjustItemTransactionsResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAdjustTransactionAsyc()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetAdjustItemTransactionsResponseDTO>> res = await Mediator.Send(new GetAdjustItemTransactionsQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetAdjustTransactionAsyc Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/adjusttransaction/{adjusttransactionid:int}")]
    [ProducesResponseType(typeof(GetAdjustItemTransactionByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAdjustTransactionByIDAsyc(int adjusttransactionid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetAdjustItemTransactionByIDResponseDTO> res = await Mediator.Send(new GetAdjustItemTransactionByIDQuery { adjusttransactionid = adjusttransactionid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetAdjustTransactionByIDAsyc Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/adjusttransactionbybranchid/{branchid:int}")]
    [ProducesResponseType(typeof(List<GetAdjustItemTransactionsResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAdjustTransactionByBranchIDAsyc(int branchid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetAdjustItemTransactionsResponseDTO>> res = await Mediator.Send(new GetAdjustItemTransactionByBranchIDQuery { branchid = branchid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetAdjustTransactionByBranchIDAsyc Success");
        return Ok(res.data);
    }


}
