using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.SubItemTypeAPI;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.ComponentService.Web.Controllers;
public class SubItemTypeController : BaseController
{
    private readonly ISubItemTypeAPI _subItemTypeAPI;
    public SubItemTypeController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        ISubItemTypeAPI subItemTypeAPI) : base(httpClientRequest, mapper, log)
    {
        _subItemTypeAPI = subItemTypeAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> SearchSubItemType([FromBody] SearchSaleBarcodeReportViewModel searchObject)
    {
        try
        {
            BaseResponse<List<GetSubItemTypeResponseDTO>> res = await _subItemTypeAPI.GetSubItemTypeListAsync();
            return View();
        }
        catch (Exception ex)
        {
            //return Json(new { data = null, message = ex.Message, recordsTotal = 0, recordsFiltered = 0 });
        }
        return View();
    }
}
