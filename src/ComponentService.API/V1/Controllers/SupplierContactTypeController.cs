using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeByID.v1;
using CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/suppliercontacttype")]
[ApiController]
public class SupplierContactTypeController : BaseApiController
{
    public SupplierContactTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
	[Route("v1/suppliercontacttypelist")]
	[ProducesResponseType(typeof(List<GetSupplierContactTypeResposeDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetSupplierContactTypeListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetSupplierContactTypeResposeDTO>> res = await Mediator.Send(new GetSupplierContactTypeListCommand());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetSupplierContactTypeListAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/suppliercontacttype/{suppliercontacttypeid:int}")]
	[ProducesResponseType(typeof(GetSupplierContactTypeResposeDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetSupplierContactTypeIDAsync(int suppliercontacttypeid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetSupplierContactTypeResposeDTO> res = await Mediator.Send(new GetSupplierContactTypeByIDCommand { suppliercontacttypeid = suppliercontacttypeid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetSupplierContactTypeIDAsync Success");
		return Ok(res.data);
	}
}