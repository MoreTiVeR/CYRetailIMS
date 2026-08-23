using Azure.Core;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
using CYRetailIMS.Application.Services.AccountService.Queries.Logout.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/account")]
public class AccountController : BaseApiController
{
    public AccountController(ILog4NetLogger log) : base(log)
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Endpoint: 
    ///     
    ///     GET: {host}/account/v1/login
    ///     Sample Request:
    /// 
    ///     {
    ///         "username": "test01"
    ///         "password": "test01"
    ///     }
    ///     
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("v1/login")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginQuery request)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<UserProfileResponseDTO> res = await Mediator.Send(request);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]LoginAsync Success");

        return Ok(res.data);
    }

	[HttpPost]
	[Route("v1/logout")]
	[ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> LogoutAsync([FromBody] LogoutQuery request)
	{
		DateTime dtStart = DateTime.Now;
		BaseResponse<CommandResponse> res = await Mediator.Send(request);
		Response.Headers.Add("responsecode", res.status);
		Response.Headers.Add("responsedatasource", res.soruce);
		Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
		_log.Debug($"[{DateTime.Now}]LogoutAsync Success");

		return Ok(res.data);
	}
}
