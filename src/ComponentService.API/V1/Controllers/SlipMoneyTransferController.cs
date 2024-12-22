using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.MoneyTransferService.Queries.GetMoneyTransferByCriteria.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Queries.GetMoneyTransferByID.v1;
using CYRetailIMS.Application.Services.MoneyTransferSlipService.Queries.GetSlipByMoneyTransferID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/slipmoneytransfer")]
public class SlipMoneyTransferController : BaseApiController
{
    public SlipMoneyTransferController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/inquirybymoneytransferid")]
    [ProducesResponseType(typeof(GetSlipByMoneyTransferIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMoneyTransferSlipByMoneyTransferIDAsync(GetSlipByMoneyTransferIDQuery reqData)
    {
        BaseResponse<GetSlipByMoneyTransferIDResponseDTO> res = await Mediator.Send(reqData);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetMoneyTransferSlipByMoneyTransferIDAsync Success");
        return Ok(res.data);
    }
}
