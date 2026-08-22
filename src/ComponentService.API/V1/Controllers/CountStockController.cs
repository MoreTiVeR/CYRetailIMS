using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CountStockService.Commands.ApproveCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.DeleteCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.SubmitCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.UpdateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockComparison.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetPendingApprovals.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/stock")]
[ApiController]
public class CountStockController : BaseApiController
{
    public CountStockController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCountStockAsync(CreateCountStockCommand createCountStockCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createCountStockCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]CreateCountStockAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/update")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCountStockAsync(UpdateCountStockCommand updateCountStockCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(updateCountStockCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateCountStockAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/delete")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCountStockAsync(DeleteCountStockCommand deleteCountStockCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(deleteCountStockCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]DeleteCountStockAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/inquiry")]
    [ProducesResponseType(typeof(List<InquiryCountStockResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCountStockAsync(InquiryCountStocksQuery createItemCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<InquiryCountStockResponseDTO>> res = await Mediator.Send(createItemCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetAllCountStockAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/inquiry-countstock-bybranch")]
    [ProducesResponseType(typeof(List<InquiryCountStockByBranchIDResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountStockByBranchAsync(InquiryCountStockByBranchIDQuery inquiryCountStockByBranchID)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>> res = await Mediator.Send(inquiryCountStockByBranchID);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetCountStockByBranchAsync Success");
        return Ok(res.data);
    }

    //InquiryCountStockByIDQuery
    [HttpPost]
    [Route("v1/inquiry-countstock-byid")]
    [ProducesResponseType(typeof(InquiryCountStockByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountStockByIDAsync(InquiryCountStockByIDQuery inquiryCountStockByID)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<InquiryCountStockByIDResponseDTO> res = await Mediator.Send(inquiryCountStockByID);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetCountStockByIDAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/submit")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SubmitCountStockAsync(SubmitCountStockCommand submitCountStockCommand)
    {
        BaseResponse<CommandResponse> res = await Mediator.Send(submitCountStockCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]SubmitCountStockAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/approve")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ApproveCountStockAsync(ApproveCountStockCommand approveCountStockCommand)
    {
        BaseResponse<CommandResponse> res = await Mediator.Send(approveCountStockCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]ApproveCountStockAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/pending-approvals")]
    [ProducesResponseType(typeof(List<GetPendingApprovalsResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPendingApprovalsAsync(GetPendingApprovalsQuery query)
    {
        BaseResponse<List<GetPendingApprovalsResponseDTO>> res = await Mediator.Send(query);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetPendingApprovalsAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/comparison")]
    [ProducesResponseType(typeof(List<GetCountStockComparisonResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountStockComparisonAsync(GetCountStockComparisonQuery query)
    {
        BaseResponse<List<GetCountStockComparisonResponseDTO>> res = await Mediator.Send(query);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetCountStockComparisonAsync Success");
        return Ok(res.data);
    }
}
