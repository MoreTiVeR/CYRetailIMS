using AutoMapper;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.ExternalService.CurrencyAPI;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.ExternalService.PaymentTypeAPI;
using CYRetailIMS.Application.ExternalService.PurchaseOrderAPI;
using CYRetailIMS.Application.ExternalService.PurchaseTypeAPI;
using CYRetailIMS.Application.ExternalService.ShipmentTypeAPI;
using CYRetailIMS.Application.ExternalService.SupplierAPI;
using CYRetailIMS.Application.ExternalService.SupplierContactTypeAPI;
using CYRetailIMS.Application.ExternalService.WarehouseAPI;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Infrastructure.ExternalService.ItemAPI;
using Microsoft.AspNetCore.Mvc;
using static CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize.CustomAuthorize;

namespace CYRetailIMS.ComponentService.Web.Controllers;

[CustomAuthorize(RoleName.Admin, RoleName.Sale, RoleName.AccountingOfficer, RoleName.AreaSale)]
public class OrderController : BaseController
{
    private readonly IPurchaseOrderAPI _purchaseOrderAPI;
    private readonly IItemAPI _itemAPI;
    private readonly ICurrencyAPI _currencyAPI;
    private readonly IPaymentTypeAPI _paymentTypeAPI;
    private readonly IPurchaseTypeAPI _purchaseTypeAPI;
    private readonly IShipmentTypeAPI _shipmentTypeAPI;
    private readonly ISupplierAPI _supplierAPI;
    private readonly ISupplierContactTypeAPI _supplierContactTypeAPI;
    private readonly IWarehouseAPI _warehouseAPI;

    public OrderController(IHttpClientRequest httpClientRequest, IMapper mapper, ILog4NetLogger log,
		IPurchaseOrderAPI purchaseOrderAPI,
        IItemAPI itemAPI, ICurrencyAPI currencyAPI, IPaymentTypeAPI paymentTypeAPI, 
        IPurchaseTypeAPI purchaseTypeAPI, IShipmentTypeAPI shipmentTypeAPI, 
        ISupplierAPI supplierAPI, ISupplierContactTypeAPI supplierContactTypeAPI, 
        IWarehouseAPI warehouseAPI) : base(httpClientRequest, mapper, log)
    {
        _purchaseOrderAPI = purchaseOrderAPI;
        _itemAPI = itemAPI;
        _currencyAPI = currencyAPI;
        _paymentTypeAPI = paymentTypeAPI;
        _purchaseTypeAPI = purchaseTypeAPI;
        _shipmentTypeAPI = shipmentTypeAPI;
        _supplierAPI = supplierAPI;
        _supplierContactTypeAPI = supplierContactTypeAPI;
        _warehouseAPI = warehouseAPI;
    }

    public async Task<IActionResult> Index()
    {
		BaseResponse<List<GetPurchaseOrderResposeDTO>> resOrderList = await _purchaseOrderAPI.GetPurchaseOrderListAsync();
        ViewBag.OrderList = resOrderList;
		return View();
    }
    
    public async Task<IActionResult> Create()
    {
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await _itemAPI.GetItemListAsync();
        BaseResponse<List<GetPurchaseTypeResponseDTO>> resPurchaseOrderTypeList = await _purchaseTypeAPI.GetPurchaseTypeListAsync();
        BaseResponse<List<GetPaymentTypeListResponseDTO>> resPaymentTypeList = await _paymentTypeAPI.GetPaymentTypeListAsync();
        BaseResponse<List<GetCurrencyListResponseDTO>> resCurrenctList = await _currencyAPI.GetCurrencyListAsync();
        BaseResponse<List<GetSupplierResponseDTO>> resSupplierList = await _supplierAPI.GetSupplierListAsync();

        ViewBag.ItemList = resItemList;
        ViewBag.PurchaseOrderTypeList = resPurchaseOrderTypeList;
        ViewBag.PaymentTypeList = resPaymentTypeList;
        ViewBag.CurrencyList = resCurrenctList;
        ViewBag.SupplierList = resSupplierList;
        return View();
    }

    public IActionResult Edit(int orderid)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdjustItem([FromBody] CreatePurchaseOrderViewModel createPurchaseOrderViewModel)
    {
        try
        {
            throw new NotImplementedException("Building...");
            //CreateAdjustItemCommand CreateAdjustItemCommand = MappingCreateAdjustItemCommand(adjustItemData);
            //BaseResponse<CommandResponse> res = await _adjustItemAPI.CreateAdjustItemAsync(CreateAdjustItemCommand);

            //if (res.result)
            //{
            //    //Clear TEMP_ADJUST_ITEM_DATA
            //    HttpContext.Session.Remove("TEMP_ADJUST_ITEM_DATA");
            //}
            //return Json(new { result = res.result, message = res.result ? "ปรับสต๊อกสินค้าสำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }
}
