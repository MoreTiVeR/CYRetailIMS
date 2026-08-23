
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v2;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V2.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/stock")]
public class CountStockController : ControllerBase
{
    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();

    [HttpPost]
    [Route("v2/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCountStockAsync(CreateCountStockCommand createCountStockCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createCountStockCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        //_log.Debug($"[{DateTime.Now}]CreateCountStockAsync Success");
        return Ok(res.data);
    }
}
