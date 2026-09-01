using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByCode.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByID.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/currency")]
[ApiController]
public class CurrencyController : BaseApiController
{
	public CurrencyController(ILog4NetLogger log) : base(log)
	{
	}

	[HttpGet]
	[Route("v1/currencylist")]
	[ProducesResponseType(typeof(List<GetCurrencyListResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetCurrencyListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetCurrencyListResponseDTO>> res = await Mediator.Send(new GetCurrencyListCommand());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetCurrencyListAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/currency/{currencyid:int}")]
	[ProducesResponseType(typeof(GetCurrencyByIDResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetCurrencyByIDAsync(int currencyid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetCurrencyByIDResponseDTO> res = await Mediator.Send(new GetCurrencyByIDCommand { currencyid = currencyid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetCurrencyByIDAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/currency/{currencycode}")]
	[ProducesResponseType(typeof(GetCurrencyByCodeResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetCurrencyByCodeAsync(string currencycode)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetCurrencyByCodeResponseDTO> res = await Mediator.Send(new GetCurrencyByCodeCommand { currencycode = currencycode });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetCurrencyByCodeAsync Success");
		return Ok(res.data);
	}
}
