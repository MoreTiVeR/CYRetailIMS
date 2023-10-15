using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeByID.v1;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/shipmenttype")]
[ApiController]
public class ShipmentTypeController : BaseApiController
{
    public ShipmentTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
	[Route("v1/shipmenttypelist")]
	[ProducesResponseType(typeof(List<GetShipmentTypeResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetShipmentTypeListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetShipmentTypeResponseDTO>> res = await Mediator.Send(new GetShipmentTypeListCommand());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetShipmentTypeListAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/shipmenttype/{shipmenttypeid:int}")]
	[ProducesResponseType(typeof(GetShipmentTypeResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetShipmentTypeByIDAsync(int shipmenttypeid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetShipmentTypeResponseDTO> res = await Mediator.Send(new GetShipmentTypeByIDCommand { shipmenttypeid = shipmenttypeid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetShipmentTypeByIDAsync Success");
		return Ok(res.data);
	}
}