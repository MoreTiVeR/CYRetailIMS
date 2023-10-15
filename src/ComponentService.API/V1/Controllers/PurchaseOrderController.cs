using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.ApprovePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.DeletePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.UpdatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderByID.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderByPONumber.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/purchase")]
[ApiController]
public class PurchaseOrderController : BaseApiController
{
    public PurchaseOrderController(ILog4NetLogger log) : base(log)
    {
    }

	[HttpPost]
	[Route("v1/create")]
	[ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> CreateAsync(CreatePurchaseOrderCommand purchaseOrderCommand)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<CommandResponse> res = await Mediator.Send(purchaseOrderCommand);
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]CreateAsync Success");
		return Ok(res.data);
	}

	[HttpPost]
	[Route("v1/update")]
	[ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> UpdateAsync(UpdatePurchaseOrderCommand purchaseOrderCommand)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<CommandResponse> res = await Mediator.Send(purchaseOrderCommand);
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]CreateAsync Success");
		return Ok(res.data);
	}

	[HttpPost]
	[Route("v1/delete")]
	[ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> DeleteAsync(DeletePurchaseOrderCommand deletePurchaseOrderCommand)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<CommandResponse> res = await Mediator.Send(deletePurchaseOrderCommand);
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]DeleteAsync Success");
		return Ok(res.data);
	}

	[HttpPost]
	[Route("v1/approve")]
	[ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> ApproveAsync(ApprovePurchaseOrderCommand approvePurchaseOrderCommand)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<CommandResponse> res = await Mediator.Send(approvePurchaseOrderCommand);
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]ApproveAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/purchaselist")]
	[ProducesResponseType(typeof(List<GetPurchaseOrderResposeDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetPurchaseOrderListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetPurchaseOrderResposeDTO>> res = await Mediator.Send(new GetPurchaseOrderListCommand { });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetPurchaseOrderListAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/purchase/{purchaseid:int}")]
	[ProducesResponseType(typeof(GetPurchaseOrderResposeDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetPurchaseOrderByIDAsync(int purchaseid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetPurchaseOrderResposeDTO> res = await Mediator.Send(new GetPurchaseOrderByIDCommand { purchaseorderid = purchaseid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetPurchaseOrderByIDAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/purchase/{purchaseno}")]
	[ProducesResponseType(typeof(GetPurchaseOrderResposeDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetPurchaseOrderByNumberAsync(string purchaseno)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetPurchaseOrderResposeDTO> res = await Mediator.Send(new GetPurchaseOrderByPONumberCommand { purchaseorderno = purchaseno });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetPurchaseOrderByNumberAsync Success");
		return Ok(res.data);
	}
}
