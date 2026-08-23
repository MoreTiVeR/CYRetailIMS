using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByBranchID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByCriteria.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/receipttemplate")]
[ApiController]
public class ReceiptTemplateController : BaseApiController
{
    public ReceiptTemplateController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReceiveTemplateAsync(CreateReceiveTemplateCommand createReceiveTemplate)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createReceiveTemplate);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateReceiveTemplateAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/update")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateReceiveTemplateAsync(UpdateReceiveTemplateCommand updateReceiveTemplate)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(updateReceiveTemplate);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateReceiveTemplateAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/delete")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReceiveTemplateAsync(DeleteReceiveTemplateCommand deleteReceiveTemplate)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(deleteReceiveTemplate);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]DeleteReceiveTemplateAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/templates")]
    [ProducesResponseType(typeof(List<GetReceiveTempResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTempReceiveListAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetReceiveTempResponseDTO>> res = await Mediator.Send(new GetReceiveTempListQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetTempReceiveListAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/receivetemplatebyid")]
    [ProducesResponseType(typeof(GetReceiveTempResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTempReceiveByIDAsync(GetReceiveTempByIDQuery getReceiveTempByIDQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetReceiveTempResponseDTO> res = await Mediator.Send(getReceiveTempByIDQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetTempReceiveByIDAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/receivetemplatebybranch")]
    [ProducesResponseType(typeof(GetReceiveTempResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTempReceiveByBranchIDAsync(GetReceiveTempByBranchIDQuery tempByBranchIDQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetReceiveTempResponseDTO> res = await Mediator.Send(tempByBranchIDQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetTempReceiveByBranchIDAsync Success");
        return Ok(res.data);
    }


    [HttpPost]
    [Route("v1/search")]
    [ProducesResponseType(typeof(GetReceiveTempByCriteriaResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTempReceiveByCriteriaAsync(GetReceiveTempByCriteriaQuery getReceiveTempByCriteriaQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetReceiveTempByCriteriaResponseDTO> res = await Mediator.Send(getReceiveTempByCriteriaQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetTempReceiveByCriteriaAsync Success");
        return Ok(res.data);
    }


}
