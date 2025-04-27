using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionList.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;


[Route("api/v{version:apiVersion}/transactiontype")]
[ApiController]
public class TrasnactionTypeController : BaseApiController
{
    public TrasnactionTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpPost]
    [Route("v1/inquiry")]
    [ProducesResponseType(typeof(GetItemTypeByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactionTypeByCriteriaAsync([FromBody] GetTrasnactionByCriteriaQuery reqObject)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>> res = await Mediator.Send(reqObject);
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetTransactionTypeByCriteriaAsync Success");
        return Ok(res.data);
    }
}
