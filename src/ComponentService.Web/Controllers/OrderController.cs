using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.PurchaseOrderAPI;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.AccountingOfficer, RoleName.AreaSale)]
public class OrderController : BaseController
{
    private readonly IPurchaseOrderAPI _purchaseOrderAPI;
	public OrderController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
		IPurchaseOrderAPI purchaseOrderAPI) : base(httpClientRequest, mapper, log)
    {
        _purchaseOrderAPI = purchaseOrderAPI;
    }

    public async Task<IActionResult> Index()
    {
		BaseResponse<List<GetPurchaseOrderResposeDTO>> resOrderList = await _purchaseOrderAPI.GetPurchaseOrderListAsync();
        ViewBag.OrderList = resOrderList;
		return View();
    }
    
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Edit(int orderid)
    {
        return View();
    }
}
