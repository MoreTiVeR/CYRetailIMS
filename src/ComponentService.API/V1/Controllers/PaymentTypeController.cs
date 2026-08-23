using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.PaymentTypeByID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/paymenttype")]
[ApiController]
public class PaymentTypeController : BaseApiController
{
    public PaymentTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
	[Route("v1/paymenttypelist")]
	[ProducesResponseType(typeof(List<GetPaymentTypeListResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetPaymentTypeListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetPaymentTypeListResponseDTO>> res = await Mediator.Send(new GetPaymentTypeListCommand());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetPaymentTypeListAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/paymenttype/{paymenttypeid:int}")]
	[ProducesResponseType(typeof(PaymentTypeByIDResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetPaymentTypeByIDAsync(int paymenttypeid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<PaymentTypeByIDResponseDTO> res = await Mediator.Send(new PaymentTypeByIDCommand { paymenttypeid = paymenttypeid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetPaymentTypeByIDAsync Success");
		return Ok(res.data);
	}
}