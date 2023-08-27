using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchList.v1;
using CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/userbranch")]
[ApiController]
public class UserInBranchController : BaseApiController
{
    public UserInBranchController(ILog4NetLogger log) : base(log)
    {
    }

	[HttpGet]
	[Route("v1/getuserinbranchbyuserid/{userid}")]
	[ProducesResponseType(typeof(GetUserInBranchByUserIDResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetUserInBranchByUserIDAsync(int userid)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<GetUserInBranchByUserIDResponseDTO> res = await Mediator.Send(new GetUserInBranchByUserIDQuery { userid = userid });
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]GetUserInBranchByUserIDAsync Success");
		return Ok(res.data);
	}
}
