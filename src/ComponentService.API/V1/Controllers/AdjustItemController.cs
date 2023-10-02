using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;


[Route("api/v{version:apiVersion}/adjustitem")]
[ApiController]
public class AdjustItemController : BaseApiController
{
    public AdjustItemController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/create")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AdjustItemAsync(CreateAdjustItemCommand createAdjustItemCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(createAdjustItemCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]AdjustItemAsync Success");
        return Ok(res.data);
    }

    [HttpPost]
    [Route("v1/update")]
    [ProducesResponseType(typeof(CommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAdjustItemAsync(UpdateAdjustItemCommand updateAdjustItemCommand)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<CommandResponse> res = await Mediator.Send(updateAdjustItemCommand);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]UpdateAdjustItemAsync Success");
        return Ok(res.data);
    }
}
