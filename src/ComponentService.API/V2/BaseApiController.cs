using CYRetailIMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V2;

[ApiVersion("2.0")]
[Produces("application/json")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ILog4NetLogger _log;

    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();

    public BaseApiController(ILog4NetLogger log)
    {
        _log = log;
    }
}