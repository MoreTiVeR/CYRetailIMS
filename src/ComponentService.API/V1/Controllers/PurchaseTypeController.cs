using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.PurchaseTypeByID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/purchasetype")]
[ApiController]
public class PurchaseTypeController : BaseApiController
{
    public PurchaseTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
	[Route("v1/purchasetypelist")]
	[ProducesResponseType(typeof(List<GetPurchaseTypeResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetPurchaseTypeListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetPurchaseTypeResponseDTO>> res = await Mediator.Send(new GetPurchaseTypeListCommand());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetPurchaseTypeListAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/purchasetype/{purchasetypeid:int}")]
	[ProducesResponseType(typeof(GetPurchaseTypeResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetPurchaseTypeByIDAsync(int purchasetypeid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetPurchaseTypeResponseDTO> res = await Mediator.Send(new PurchaseTypeByIDCommand { purchasetypeid = purchasetypeid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetPurchaseTypeByIDAsync Success");
		return Ok(res.data);
	}
}