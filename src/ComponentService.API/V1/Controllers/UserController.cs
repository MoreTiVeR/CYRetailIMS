using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/user")]
public class UserController : BaseApiController
{
    public UserController(ILog4NetLogger log) : base(log)
    {
    }

	[HttpPost]
	[Route("v1/create")]
	[ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> CreateUserAsync(CreateUserCommand request)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<CommandResponse> res = await Mediator.Send(request);
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]CreateUserAsync Success");

		return Ok(res.data);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpPost]
    [Route("v1/profile")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserProfiles(CreateEmployeeCommand request)
    {
        throw new NotImplementedException("NotImplementedException");
    }
}
