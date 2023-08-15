using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandByID.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/itembrand")]
[ApiController]
public class ItemBrandController : BaseApiController
{
    public ItemBrandController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("v1/getitembrandbyid/{itembrandid}")]
    [ProducesResponseType(typeof(GetItemBrandByIDResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemBrandByIDAsync(int itembrandid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetItemBrandByIDResponseDTO> res = await Mediator.Send(new GetItemBrandByIDQuery { itembrandid = itembrandid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemBrandByIDAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/getitembrandlist")]
    [ProducesResponseType(typeof(List<GetItemBrandListResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetItemBrandListAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetItemBrandListResponseDTO>> res = await Mediator.Send(new GetItemBrandListQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetItemBrandListAsync Success");
        return Ok(res.data);
    }
}
