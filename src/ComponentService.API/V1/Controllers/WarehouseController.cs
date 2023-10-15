using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseByID.v1;
using CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/warehouse")]
[ApiController]
public class WarehouseController : BaseApiController
{
    public WarehouseController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
	[Route("v1/warehouselist")]
	[ProducesResponseType(typeof(List<GetWarehouseResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetWarehouseListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetWarehouseResponseDTO>> res = await Mediator.Send(new GetWarehouseListCommand());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetWarehouseListAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/warehouse/{warehouseid:int}")]
	[ProducesResponseType(typeof(GetWarehouseResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetWarehouseByIDAsync(int warehouseid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetWarehouseResponseDTO> res = await Mediator.Send(new GetWarehouseByIDCommand { warehouseid = warehouseid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetWarehouseByIDAsync Success");
		return Ok(res.data);
	}
}