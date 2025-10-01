using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceipt.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.GenerateReceiptNo.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/receipt")]
[ApiController]
public class ReceiptController : BaseApiController
{
    public ReceiptController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/generate-receiptno")]
    [ProducesResponseType(typeof(GenerateReceiptNoResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateReceiptNoByBranchAsync(GenerateReceiptNoCommand receiptNoCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GenerateReceiptNoResponseDTO> res = await Mediator.Send(receiptNoCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GenerateReceiptNoByBranchAsync Success");
        return Ok(res.data);
    }


    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReceiptAsync(CreateReceiptCommand createReceiptCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createReceiptCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateReceiptAsync Success");
        return Ok(res.data);
    }


}
