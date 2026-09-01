using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransferList.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.UpdateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Queries.GetMoneyTransferByCriteria.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Queries.GetMoneyTransferByID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/moneytransfer")]
public class MoneyTransferController : BaseApiController
{
    public MoneyTransferController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateMoneyTransfer(CreateMoneyTransferCommand createCommand)
    {
        BaseResponse<CommandResponse> res = await Mediator.Send(createCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateMoneyTransfer Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/bulk-create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BulkCreateMoneyTransfer(CreateMoneyTransferListCommand createCommand)
    {
        BaseResponse<CommandResponse> res = await Mediator.Send(createCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]BulkCreateMoneyTransfer Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/update")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMoneyTransfer(UpdateMoneyTransferCommand updateCommand)
    {
        BaseResponse<CommandResponse> res = await Mediator.Send(updateCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateMoneyTransfer Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/delete")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMoneyTransfer(DeleteMoneyTransferCommand updateCommand)
    {
        BaseResponse<CommandResponse> res = await Mediator.Send(updateCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateMoneyTransfer Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/inquiry")]
    [ProducesResponseType(typeof(List<GetMoneyTransferByCriteriaResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMoneyTransferByCriteria(GetMoneyTransferByCriteriaQuery reqData)
    {
        BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>> res = await Mediator.Send(reqData);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetMoneyTransferByCriteria Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/inquirybyid")]
    [ProducesResponseType(typeof(GetMoneyTransferByIDQuery), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMoneyTransferByID(GetMoneyTransferByIDQuery reqData)
    {
        BaseResponse<GetMoneyTransferByCriteriaResponseDTO> res = await Mediator.Send(reqData);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetMoneyTransferByID Success");
        return Ok(res.data);
    }
}
