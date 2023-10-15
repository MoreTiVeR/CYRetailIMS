using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierByID.v1;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/supplier")]
[ApiController]
public class SupplierController : BaseApiController
{
    public SupplierController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
	[Route("v1/supplierlist")]
	[ProducesResponseType(typeof(List<GetSupplierResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetSupplierListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetSupplierResponseDTO>> res = await Mediator.Send(new GetSupplierListCommand());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetSupplierListAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/supplier/{supplierid:int}")]
	[ProducesResponseType(typeof(GetSupplierResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetSupplierByIDAsync(int supplierid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetSupplierResponseDTO> res = await Mediator.Send(new GetSupplierByIDCommand { supplierid = supplierid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetSupplierByIDAsync Success");
		return Ok(res.data);
	}
}