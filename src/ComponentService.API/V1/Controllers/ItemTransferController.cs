using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByDraftID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v2;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransferFromDraft.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.DeleteDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByCriteria.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.ValidatePrintDraftItemTransferByDraftID.v1;
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
    [Route("v2/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateItemTransferV2Async(CreateItemTransferWithDraftCommand createItemTransferCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createItemTransferCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateItemTransferV2Async Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/draft")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DraftItemTransferAsync(CreateDraftItemTransferCommand createDraftItemTransferCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createDraftItemTransferCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]DraftItemTransferAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/deletedraft")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDraftItemTransferAsync(DeleteDraftItemTransferCommand deleteDraftItemTransferCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(deleteDraftItemTransferCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]DeleteDraftItemTransferAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/createbydraft")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateItemTransferFromDraftCommandAsync(CreateItemTransferFromDraftCommand reqObj)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(reqObj);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateItemTransferFromDraftCommandAsync Success");
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

    [HttpPost]
    [Route("v1/itemtransferforadmin")]
    [ProducesResponseType(typeof(GetItemTransferListResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferForAdminAsync(GetItemTransferListQuery getItemTransferListQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetItemTransferListResponseDTO> res = await Mediator.Send(getItemTransferListQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemTransferForAdminAsync Success");
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
    [ProducesResponseType(typeof(GetItemTransferListResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemTransferByDestinationBranchIDAsync(GetItemTransferByDestinationBranchIDQuery getItemTransferByDestinationBranchIDQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetItemTransferListResponseDTO> res = await Mediator.Send(getItemTransferByDestinationBranchIDQuery);
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

    [HttpPost]
    [Route("v1/inquiry-draft-itemtransfer-branch")]
    [ProducesResponseType(typeof(GetDraftItemTransferByBranchIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDraftItemTransferByBranchIDAsync(GetDraftItemTransferByBranchIDQuery getDraftItemTransferQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetDraftItemTransferByBranchIDResponseDTO> res = await Mediator.Send(getDraftItemTransferQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetDraftItemTransferByBranchIDAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/inquiry-draft-itemtransfer")]
    [ProducesResponseType(typeof(List<GetDraftItemTransferByBranchIDResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDraftItemTransferByCriteriaAsync(GetDraftItemTransferByCriteriaQuery itemTransferByCriteriaQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>> res = await Mediator.Send(itemTransferByCriteriaQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetDraftItemTransferByCriteriaAsync Success");
        return Ok(res.data);
    }

    
    [HttpPost]
    [Route("v1/inquiry-draftid")]
    [ProducesResponseType(typeof(List<GetItemInventoryTransferResposeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemInventoryForTransferByDraftIDQueryAsync(GetItemInventoryForTransferByDraftIDQuery reqObj)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetItemInventoryTransferResposeDTO>> res = await Mediator.Send(reqObj);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemInventoryForTransferByDraftIDQueryAsync Success");
        return Ok(res.data);
    }
    
    [HttpPost]
    [Route("v1/update-draft")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDraftItemTransferAsync(UpdateDraftItemTransferCommand reqObj)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(reqObj);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateDraftItemTransferAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/validate-draft-printable")]
    [ProducesResponseType(typeof(ValidatePrintDraftItemTransferResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidatePrintDraftItemTransAsync(ValidatePrintDraftItemTransferQuery reqObj)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<ValidatePrintDraftItemTransferResponseDTO> res = await Mediator.Send(reqObj);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]ValidatePrintDraftItemTransAsync Success");
        return Ok(res.data);
    }

}
