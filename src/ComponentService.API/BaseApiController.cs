using CYRetailIMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API;

[ApiVersion("1.0")]
[Produces("application/json")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    //private ILog4NetLogger _log = null!;
    protected readonly ILog4NetLogger _log;

    private ISender _mediator = null!;
    protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetRequiredService<ISender>();

    public BaseApiController(ILog4NetLogger log)
    {
        _log = log;
    }
}