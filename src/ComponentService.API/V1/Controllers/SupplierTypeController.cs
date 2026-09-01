using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
using CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;
using CYRetailIMS.Application.Services.SupplierTypeService.Queries.SupplierTypeByID.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;

[Route("api/v{version:apiVersion}/suppliertype")]
public class SupplierTypeController : BaseApiController
{
    public SupplierTypeController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("v1/suppliertypes")]
    [ProducesResponseType(typeof(List<GetSupplierTypeResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSupplierTypesAsync()
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<List<GetSupplierTypeResponseDTO>> res = await Mediator.Send(new GetSupplierTypeListQuery());
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetSupplierTypesAsync Success");
        return Ok(res.data);
    }

    [HttpGet]
    [Route("v1/suppliertype/{suppliertypeid:int}")]
    [ProducesResponseType(typeof(GetSupplierTypeResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSupplierTypeByIDAsync(int suppliertypeid)
    {
        DateTime dtStart = DateTime.Now;
        BaseResponse<GetSupplierTypeResponseDTO> res = await Mediator.Send(new SupplierTypeByIDQuery { suppliertypeid = suppliertypeid });
        Response.Headers.Add("responsecode", res.status);
        Response.Headers.Add("responsedatasource", res.soruce);
        Response.Headers.Add("responsemessage", res.message?.Replace(Environment.NewLine, string.Empty));
        _log.Debug($"[{DateTime.Now}]GetSupplierTypeByIDAsync Success");
        return Ok(res.data);
    }
}
