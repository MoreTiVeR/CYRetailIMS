using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatusByID.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;
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

    [HttpPost]
    [Route("v1/receive")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReceiveItemTransferAsync(UpdateItemTransferCommand updateItemTransferCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(updateItemTransferCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]ReceiveItemTransferAsync Success");
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
        BaseResponse<GetItemTransferResponseDTO> res = await Mediator.Send(new GetItemTransferByTransferIDQuery { transferid = transferid });
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

    [HttpPost]
    [Route("v1/itemtransferbycriteria")]
    [ProducesResponseType(typeof(List<GetItemTransferResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferByCriteriaAsync(GetItemTransferByCriteriaQuery getItemTransferByCriteriaQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetItemTransferResponseDTO>> res = await Mediator.Send(getItemTransferByCriteriaQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTransferByCriteriaAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/itemtransferbybranchid")]
    [ProducesResponseType(typeof(List<GetItemTransferResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferByDestinationBranchIDAsync(GetItemTransferByDestinationBranchIDQuery getItemTransferByDestinationBranchIDQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetItemTransferResponseDTO>> res = await Mediator.Send(getItemTransferByDestinationBranchIDQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTransferByCriteriaAsync Success");
        return Ok(res.data);
    }

    #region TMItemTransferStatus
    [HttpGet]
    [Route("v1/itemtransferstatus")]
    [ProducesResponseType(typeof(List<GetItemTransferStatusResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferStatusAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetItemTransferStatusResponseDTO>> res = await Mediator.Send(new GetItemTransferStatusQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTransferStatusAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/itemtransferstatusbyid/{transferstatusid:int}")]
    [ProducesResponseType(typeof(GetItemTransferStatusByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferStatusByIDAsync(int transferstatusid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetItemTransferStatusByIDResponseDTO> res = await Mediator.Send(new GetItemTransferStatusByIDQuery { transferstatusid = transferstatusid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTransferStatusByIDAsync Success");
        return Ok(res.data);
    }

    #endregion

    #region TMTransferType
    [HttpGet]
    [Route("v1/itemtransfertype")]
    [ProducesResponseType(typeof(List<GetTransferTypeListResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferTypeAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetTransferTypeListResponseDTO>> res = await Mediator.Send(new GetTransferTypeListQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTransferTypeAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/itemtransfertypebyid/{transfertypeid:int}")]
    [ProducesResponseType(typeof(GetTransferTypeByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferTypeByIDAsync(int transfertypeid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetTransferTypeByIDResponseDTO> res = await Mediator.Send(new GetTransferTypeByIDQuery { transfertypeid = transfertypeid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTransferTypeByIDAsync Success");
        return Ok(res.data);
    }
    #endregion

}
