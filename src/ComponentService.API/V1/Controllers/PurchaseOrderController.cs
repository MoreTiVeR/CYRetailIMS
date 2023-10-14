using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
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
}
