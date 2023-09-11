using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/itemtransfer")]
[ApiController]
public class ItemTransferController : BaseApiController
{
    public ItemTransferController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateItemTransferAsync(CreateItemTransferCommand createItemTransferCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createItemTransferCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateItemTransferAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/itemtransfer/{transferid}")]
    [ProducesResponseType(typeof(GetItemTransferResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferByIDAsync(int transferid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetItemTransferResponseDTO> res = await Mediator.Send(new GetItemTransferByTransferIDQuery { transferid = transferid});
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateItemTransferAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/itemtransferlist")]
    [ProducesResponseType(typeof(List<GetItemTransferResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferListAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetItemTransferResponseDTO>> res = await Mediator.Send(new GetItemTransferListQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTransferListAsync Success");
        return Ok(res.data);
    }
}
