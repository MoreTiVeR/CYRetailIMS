using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Staff)]
public class SaleController : BaseController
{
    private readonly IItemInBranchAPI _itemInBranchAPI;
    public SaleController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
        IItemInBranchAPI itemInBranchAPI) : base(httpClientRequest, mapper, log)
    {
        _itemInBranchAPI = itemInBranchAPI;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Create()
    {
        BaseResponse<GetItemInBranchByBranchIDResponseDTO> resItemInBranch = await _itemInBranchAPI.GetItemInBranchByBranchIDAsync(UserProfile.access_branch.FirstOrDefault().branchid);
        ViewBag.ItemInBranch = resItemInBranch.data.itemlist;
        return View();
    }

	public IActionResult Items()
	{
		return View();
	}
}
