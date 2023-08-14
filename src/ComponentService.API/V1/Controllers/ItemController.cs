using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/item")]
[ApiController]
public class ItemController : BaseApiController
{
    public ItemController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("v1/getitembyid/{itemid}")]
    [ProducesResponseType(typeof(GetItemByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemByIDAsync(int itemid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetItemByIDResponseDTO> res = await Mediator.Send(new GetItemByIDQuery { itemid = itemid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemByIDAsync Success");
        return Ok(res.data);
    }
}