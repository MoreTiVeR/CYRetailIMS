using CYRetailIMS.Application.Common.Interfaces;
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
}
