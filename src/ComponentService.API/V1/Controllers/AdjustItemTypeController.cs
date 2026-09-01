using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemTypeByID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/adjustitemtype")]
[ApiController]
public class AdjustItemTypeController : BaseApiController
{
    public AdjustItemTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("v1/getadjusttypes")]
    [ProducesResponseType(typeof(List<GetAdjustItemTypeResposeDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAdjustTypeAsyc()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetAdjustItemTypeResposeDTO>> res = await Mediator.Send(new GetAdjustItemTypeQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetAdjustTypeAsyc Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/getadjusttype/{adjusttypeid:int}")]
    [ProducesResponseType(typeof(List<GetAdjustItemTypeByIDResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAdjustTypeByIDAsyc(int adjusttypeid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetAdjustItemTypeByIDResponseDTO>> res = await Mediator.Send(new GetAdjustItemTypeByIDQuery { adjusttypeid = adjusttypeid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetAdjustTypeByIDAsyc Success");
        return Ok(res.data);
    }
}
