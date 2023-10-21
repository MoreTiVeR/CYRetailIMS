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
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.DeletePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.UpdatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;
using CYRetailIMS.ComponentService.Web.Common.Infrasructure.Authorize;
using CYRetailIMS.Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NUglify.Helpers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
    private string _sessionTempDataName => "TEMP_ORDER_ITEM_DATA";

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
		//BaseResponse<List<GetPurchaseOrderResposeDTO>> resOrderList = await _purchaseOrderAPI.GetPurchaseOrderListAsync();
        //ViewBag.OrderList = resOrderList;
		return View();
    }
    
    public async Task<IActionResult> Create()
    {
        #region Get- Set Item
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await GetItemSessionDataAsync();
        #endregion

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

    public async Task<IActionResult> Edit(int orderid)
    {
        BaseResponse<GetPurchaseOrderResposeDTO> resPurchaseOrder = await _purchaseOrderAPI.GetPurchaseOrderByIDAsync(orderid);
        EditPurchaseOrderViewModel editPurchaseViewModel = MappingEditViewData(resPurchaseOrder.data);

        #region Get- Set Item
        BaseResponse<List<GetItemListResponseDTO>> resItemList = await GetItemSessionDataAsync();
        #endregion
        BaseResponse<List<GetPurchaseTypeResponseDTO>> resPurchaseOrderTypeList = await _purchaseTypeAPI.GetPurchaseTypeListAsync();
        BaseResponse<List<GetPaymentTypeListResponseDTO>> resPaymentTypeList = await _paymentTypeAPI.GetPaymentTypeListAsync();
        BaseResponse<List<GetCurrencyListResponseDTO>> resCurrenctList = await _currencyAPI.GetCurrencyListAsync();
        BaseResponse<List<GetSupplierResponseDTO>> resSupplierList = await _supplierAPI.GetSupplierListAsync();

        ViewBag.ItemList = resItemList;
        ViewBag.PurchaseOrderTypeList = resPurchaseOrderTypeList;
        ViewBag.PaymentTypeList = resPaymentTypeList;
        ViewBag.CurrencyList = resCurrenctList;
        ViewBag.SupplierList = resSupplierList;
        return View(editPurchaseViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrderItem([FromBody] CreatePurchaseOrderViewModel createPurchaseOrderData)
    {
        try
        {
            CreatePurchaseOrderCommand createPurchaseOrderCommand = MappingCreatePurchaseOrder(createPurchaseOrderData);
            BaseResponse<CommandResponse> res = await _purchaseOrderAPI.CreatePurchaseOrderAsync(createPurchaseOrderCommand);
            if (res.result)
            {
                //Clear TEMP_ADJUST_ITEM_DATA
                HttpContext.Session.Remove(_sessionTempDataName);
            }
            return Json(new { result = res.result, message = res.result ? "สร้างใบสั่งซื้อสำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePurchaseOrderItem([FromBody] EditPurchaseOrderViewModel updatetPurchaseOrderData)
    {
        try
        {
            UpdatePurchaseOrderCommand updatePurchaseOrderCommand = MappingUpdatePurchaseOrder(updatetPurchaseOrderData);
            BaseResponse<CommandResponse> res = await _purchaseOrderAPI.UpdatePurchaseOrderAsync(updatePurchaseOrderCommand);
            //if (res.result)
            //{
            //    //Clear TEMP_ADJUST_ITEM_DATA
            //    HttpContext.Session.Remove(_sessionTempDataName);
            //}
            return Json(new { result = res.result, message = res.result ? "ปรับปรุงข้อมูลใบสั่งซื้อสำเร็จ" : $"ไม่สามารถทำรายการได้, {res.error.error.message}" });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteItem([FromBody] DeletePurchaseOrderViewModel delPurchaseOrder)
    {
        try
        {
            DeletePurchaseOrderCommand delItemCommand = new DeletePurchaseOrderCommand
            {
                purchaseorderid = delPurchaseOrder.purchaseorderid,
                deletedby = base.UserProfile.rolename,
                deleteddate = DateTime.Now
            };
            BaseResponse<CommandResponse> resDelItem = await _purchaseOrderAPI.DeletePurchaseOrderAsync(delItemCommand);
            if (resDelItem.result)
            {
                return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.message });
            }
            return Json(new JsonViewModel { result = resDelItem.result, message = resDelItem.error.error.message });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });

        }
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetTempItem()
    {
        try
        {
            List<PurchaseOrderItemViewModel> tempList = HttpContext.Session.GetDataFromSession<List<PurchaseOrderItemViewModel>>(_sessionTempDataName);

            #region if list is null => create new list with 0 member
            if (tempList == null)
            {
                tempList = new List<PurchaseOrderItemViewModel>();
                HttpContext.Session.SetDataToSession(_sessionTempDataName, tempList);
            }
            #endregion
            return Json(new { data = tempList.OrderBy(o => o.nseq).ToList() });
        }
        catch
        {
            return Json(new { data = new List<PurchaseOrderItemViewModel>() });
        }

    }

    [HttpPost]
    public IActionResult AddTempItem([FromBody] PurchaseOrderItemViewModel orderItemData)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (orderItemData.nqty < 0)
            {
                throw new Exception("กรุณาระบุจำนวนไม่น้อยกว่า 0");
            }

            //Get Current List
            List<PurchaseOrderItemViewModel> tempOrderItemList = HttpContext.Session.GetDataFromSession<List<PurchaseOrderItemViewModel>>(_sessionTempDataName);

            #region Update when Already added
            var existData = tempOrderItemList.FirstOrDefault(w => w.nitemid == orderItemData.nitemid);
            if (existData != null)
            {
                //Update QTY
                tempOrderItemList.Where(w =>  w.nitemid == orderItemData.nitemid).ForEach(e =>
                {
                    e.nqty = e.nqty + orderItemData.nqty;
                });
            }
            else
            {
                //Add new
                int lastId = tempOrderItemList != null && tempOrderItemList.Count > 0 ? tempOrderItemList.Last().nseq : 0;
                lastId++;
                orderItemData.nseq = lastId;
                MappingPurchaseOrderItem(ref orderItemData);
                tempOrderItemList.Add(orderItemData);
            }
            #endregion

            HttpContext.Session.SetDataToSession(_sessionTempDataName, tempOrderItemList);
            return Json(new { result = true, message = "เพิ่มสินค้าสั่งซื้อสำเร็จ" });

        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }

    [HttpPost]
    public JsonResult DeleteTempItem(int seq)
    {
        try
        {
            List<PurchaseOrderItemViewModel> res = HttpContext.Session.GetDataFromSession<List<PurchaseOrderItemViewModel>>(_sessionTempDataName);
            PurchaseOrderItemViewModel todo = res?.FirstOrDefault(m => m.nseq == seq);
            if (todo == null)
            {
                throw new Exception("ไม่สามารถลบข้อมูลได้");
            }

            res.Remove(todo);
            HttpContext.Session.SetDataToSession(_sessionTempDataName, res);
            return Json(new { result = true, message = "Delete success." });
        }
        catch (Exception ex)
        {
            return Json(new { result = false, message = $"พบข้อผิดพลาด {ex.Message}" });
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        BaseResponse<List<GetPurchaseOrderResposeDTO>> resOrderList = await _purchaseOrderAPI.GetPurchaseOrderListAsync();
        return Json(new { data = resOrderList.data });
    }


    private void MappingPurchaseOrderItem(ref PurchaseOrderItemViewModel orderItem)
    {
        BaseResponse<List<GetItemListResponseDTO>> resItems = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetItemListResponseDTO>>>("ITEM_DATA");        
        int refItemID = orderItem.nitemid;
        orderItem.sitemname = resItems.data.FirstOrDefault(w => w.itemid == refItemID).name;
    }

    private async Task<BaseResponse<List<GetItemListResponseDTO>>> GetItemSessionDataAsync()
    {
        BaseResponse<List<GetItemListResponseDTO>> res = HttpContext.Session.GetDataFromSession<BaseResponse<List<GetItemListResponseDTO>>>("ITEM_DATA");
        if (res != null)
        {
            return res;
        }
        res = await _itemAPI.GetItemListAsync();
        HttpContext.Session.SetDataToSession("ITEM_DATA", res);
        return res;
    }

    private CreatePurchaseOrderCommand MappingCreatePurchaseOrder(CreatePurchaseOrderViewModel orderViewModel)
    {
        List<PurchaseOrderItemViewModel> tempList = HttpContext.Session.GetDataFromSession<List<PurchaseOrderItemViewModel>>(_sessionTempDataName);

        orderViewModel.createdby = base.UserProfile.rolename;
        orderViewModel.createddate = DateTime.Now;
        CreatePurchaseOrderCommand purchaseOrderRequest = new CreatePurchaseOrderCommand
        {
            purchasetypeid = orderViewModel.npurchasetypeid,
            paymentypeid = orderViewModel.npaymenttypeid,
            supplierid = orderViewModel.nsupplierid,
            currencyid = orderViewModel.ncurrencyid == 0 ? 1 : orderViewModel.ncurrencyid, //THB
            orderdate = orderViewModel.createddate.Value,
            receiveddate = null,
            remarks = orderViewModel.Remark,
            amount = orderViewModel.amount,
            discount = orderViewModel.discount,
            subtotal = orderViewModel.amount - orderViewModel.discount,
            tax = 0,
            total = (orderViewModel.amount - orderViewModel.discount) - 0,
            createdby = orderViewModel.createdby,
            createddate = orderViewModel.createddate.Value,
            isactive = true,
            approvestatus = (int)EnumModel.ApproveStatus.WaitingApprove,
            shipment = new CreateShipmentCommand
            {
                shipmenttypeid = 1,
                shipmentname = "Delivery Express",
                shipmentdate = orderViewModel.createddate.Value,
                warehouseid = 1,
                trackingno = !string.IsNullOrEmpty(orderViewModel.trackingno) ? orderViewModel.trackingno : null
            },
            detail = (from a in tempList
                      select new CreatePurchaseOrderDetailCommand
                      {
                          itemid = a.nitemid,
                          qty = a.nqty,
                          price = a.price,
                          amount = a.amount,
                          discountpercentage = 0,
                          discountamount = 0,
                          taxpercentage = 0,
                          taxamount = 0,
                          //subtotal = autocalculate
                          //total = autocalculate
                      }).ToList()

        };
        return purchaseOrderRequest;
    }

    private UpdatePurchaseOrderCommand MappingUpdatePurchaseOrder(EditPurchaseOrderViewModel orderViewModel)
    {
        List<PurchaseOrderItemViewModel> tempList = HttpContext.Session.GetDataFromSession<List<PurchaseOrderItemViewModel>>(_sessionTempDataName);

        UpdatePurchaseOrderCommand updatePurchase = new UpdatePurchaseOrderCommand
        {
            purchaseorderid = orderViewModel.purchaseorderid,
            isactive = true,
            approvestatus = orderViewModel.approvestatus,
            trackingno = !string.IsNullOrEmpty(orderViewModel.trackingno) ? orderViewModel.trackingno : null,
            updatedby = base.UserProfile.rolename,
            updateddate = DateTime.Now,
            detail = (from a in tempList
                      select new CreatePurchaseOrderDetailCommand
                      {
                          itemid = a.nitemid,
                          qty = a.nqty,
                          price = a.price,
                          amount = a.amount,
                          discountpercentage = 0,
                          discountamount = 0,
                          taxpercentage = 0,
                          taxamount = 0,
                          //subtotal = autocalculate
                          //total = autocalculate
                      }).ToList()
        };
        return updatePurchase;
    }

    private EditPurchaseOrderViewModel MappingEditViewData(GetPurchaseOrderResposeDTO orderResposeDTO)
    {
        EditPurchaseOrderViewModel editPurchase = new EditPurchaseOrderViewModel
        {
            purchaseorderid = orderResposeDTO.purchaseorderid,
            npurchasetypeid = orderResposeDTO.purchasetypeid,
            npaymenttypeid = orderResposeDTO.paymentypeid,
            nsupplierid = orderResposeDTO.supplierid,
            ncurrencyid = orderResposeDTO.currencyid,
            Remark = orderResposeDTO.remarks,
            discount = orderResposeDTO.discount,
            amount = orderResposeDTO.amount,
            total = orderResposeDTO.total,
            trackingno = orderResposeDTO.shipment.trackingno,
            createdby = orderResposeDTO.createdby,
            createddate = orderResposeDTO.creadeddate,
            approvestatus = orderResposeDTO.approvestatus
        };

        //Set OrderDetail to session temp data
        int seq = 1;
        List<PurchaseOrderItemViewModel> purchaseOrders = (from a in orderResposeDTO.detail
                                                           select new PurchaseOrderItemViewModel
                                                           {
                                                               nseq = seq++,
                                                               nitemid = a.itemid,
                                                               sitemname = a.itemname,
                                                               nqty = a.quantity,
                                                               price = a.price,
                                                               amount = a.amount
                                                           }).ToList();
        HttpContext.Session.SetDataToSession(_sessionTempDataName, purchaseOrders);

        return editPurchase;
    }
}
