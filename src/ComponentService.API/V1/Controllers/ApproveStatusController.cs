using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ApproveStatusService.Queries.GetApproveStatus.v1;
using CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/approvestatus")]
[ApiController]
public class ApproveStatusController : BaseApiController
{
    public ApproveStatusController(ILog4NetLogger log) : base(log)
    {
    }

	[HttpGet]
	[Route("v1/getapprovestatus")]
	[ProducesResponseType(typeof(List<GetApproveStatusResponseDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetApproveStatusAsyc()
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<List<GetApproveStatusResponseDTO>> res = await Mediator.Send(new GetApproveStatusQuery());
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetApproveStatusAsyc Success");
		return Ok(res.data);
	}
}
