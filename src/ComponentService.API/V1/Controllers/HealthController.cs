using System.Reflection;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.API.V1.Controllers;


[Route("api/v{version:apiVersion}")]
public class HealthController : BaseApiController
{
    public HealthController(ILog4NetLogger log) : base(log)
    {
    }

    [HttpGet]
    [Route("health")]
    public IActionResult Alive() => Ok(new HealthCheckResponse { version = Assembly.GetEntryAssembly()?.GetName().Version.ToString() });
}
