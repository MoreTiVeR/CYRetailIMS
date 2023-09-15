using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/itembranch")]
[ApiController]
public class ItemBranchController : BaseApiController
{
    public ItemBranchController(ILog4NetLogger log) : base(log)
    {
    }

	[HttpGet]
	[Route("v1/getiteminbranchbybranchid/{branchid}")]
	[ProducesResponseType(typeof(GetItemInBranchByBranchIDResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetItemInBranchByIDAsync(int branchid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetItemInBranchByBranchIDResponseDTO> res = await Mediator.Send(new GetItemInBranchByBranchIDQuery { branchid = branchid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetItemInBranchByIDAsync Success");
		return Ok(res.data);
	}

	[HttpGet]
	[Route("v1/getiteminbranchlist")]
	[ProducesResponseType(typeof(List<GetItemInBranchListResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetItemInBranchListAsync()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetItemInBranchListResponseDTO>> res = await Mediator.Send(new GetItemInBranchListQuery());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetItemInBranchListAsync Success");
		return Ok(res.data);
	}

	[HttpPost]
	[Route("v1/getiteminbranchbybranchidlist")]
	[ProducesResponseType(typeof(List<GetItemInBranchByBranchListResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetItemInBranchByBranchIDListAsync(GetItemInBranchByBranchListQuery itemInBranchByBranchListQuery)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetItemInBranchByBranchListResponseDTO>> res = await Mediator.Send(itemInBranchByBranchListQuery);
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetItemInBranchByBranchIDListAsync Success");
		return Ok(res.data);
	}

    [HttpPost]
    [Route("v1/getiteminbranchbycriteria")]
    [ProducesResponseType(typeof(GetItemInBranchByCriteriaResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemInBranchByCriteriaAsync(GetItemInBranchByCriteriaQuery itemInBranchByBranchListQuery)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetItemInBranchByCriteriaResponseDTO> res = await Mediator.Send(itemInBranchByBranchListQuery);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemInBranchByCriteriaAsync Success");
        return Ok(res.data);
    }
}
