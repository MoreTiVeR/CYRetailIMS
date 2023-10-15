using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderByID.v1;
public class GetPurchaseOrderByIDHandler : BaseService, IRequestHandler<GetPurchaseOrderByIDCommand, BaseResponse<GetPurchaseOrderResposeDTO>>
{
    public GetPurchaseOrderByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetPurchaseOrderResposeDTO>> Handle(GetPurchaseOrderByIDCommand request, CancellationToken cancellationToken)
    {
		List<GetPurchaseOrderResposeDTO> resOrder = (from order in await _unitOfWork.Repository<TTPurchaseOrder>().FindWithInclude(w => w.PurchaseOrderID == request.purchaseorderid 
													 && w.IsActive, 
													 i => i.Include(w => w.TTPurchaseOrderDetails), 
													 ii => ii.Include(w => w.TTShipments).ThenInclude(w => w.Warehouse))
													 join ordertype in await _unitOfWork.Repository<TMPurchaseType>().QueryAsync() on order.PurchaseTypeID equals ordertype.PurchaseTypeID
													 join supp in await _unitOfWork.Repository<TMSupplier>().QueryAsync() on order.SupplierID equals supp.SupplierID
													 join cur in await _unitOfWork.Repository<TMCurrency>().QueryAsync() on order.CurrencyID equals cur.CurrencyID
													 join paytype in await _unitOfWork.Repository<TMPaymentType>().QueryAsync() on order.PaymenTypeID equals paytype.PaymenTypeID
													 select new GetPurchaseOrderResposeDTO
													 {
														 purchaseorderid = order.PurchaseOrderID,
														 purchaseorderno = order.PurchaseOrderNo,
														 currencyid = order.CurrencyID,
														 currencyname = cur.CurrencyName,
														 paymentypeid = order.PaymenTypeID,
														 paymentypename = paytype.PaymenTypeName,
														 purchasetypeid = order.PurchaseTypeID,
														 purchasetypename = ordertype.PurchaseTypeName,
														 supplierid = order.SupplierID,
														 suppliername = supp.SupplierName_TH,
														 remarks = order.Remarks,
														 subtotal = order.SubTotal,
														 tax = order.Tax,
														 discount = order.Discount,
														 amount = order.Amount,
														 total = order.Total,
														 orderdate = order.OrderDate,
														 receiveddate = order.ReceivedDate,
														 createdby = order.CreatedBy,
														 creadeddate = order.CreadedDate,
														 isactive = order.IsActive,
														 approvestatus = order.ApproveStatus,
														 shipment = (from a in order.TTShipments
																	 select new GetPurchaseOrderShipmentResponseDTO
																	 {
																		 shipmentid = a.ShipmentID,
																		 shipmentname = a.ShipmentName,
																		 shipmentdate = a.ShipmentDate,
																		 shipmenttypeid = a.ShipmentID,
																		 shipmenttypename = a.ShipmentType.ShipmentTypeName,
																		 warehouseid = a.WarehouseID,
																		 warehousename = a.Warehouse.WarehouseName,
																		 trackingno = a.TrackingNo,
																		 createdby = a.CreatedBy,
																		 creadeddate = a.CreadedDate,
																		 isactive = a.IsActive
																	 }).FirstOrDefault(),
														 detail = (from a in order.TTPurchaseOrderDetails
																   select new GetPurchaseOrderDetailResponseDTO
																   {
																	   purchaseorderdetailid = a.PurchaseOrderDetailID,
																	   itemid = a.ItemID,
																	   itemname = string.Empty,
																	   description = a.Description,
																	   price = a.Price,
																	   quantity = a.Quantity,
																	   amount = a.Amount,
																	   discountamount = a.DiscountAmount,
																	   discountpercentage = a.DiscountPercentage,
																	   taxamount = a.TaxAmount,
																	   taxpercentage = a.TaxPercentage,
																	   subtotal = a.SubTotal,
																	   total = a.Total,
																	   isactive = a.IsActive
																   }).ToList()
													 }).ToList();
		if (!resOrder.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}

		#region Prepare ItemName
		List<int> itemidList = resOrder.SelectMany(s => s.detail).Select(s => s.itemid).Distinct().ToList();
		IEnumerable<TMItem> resItemDList = await _unitOfWork.Repository<TMItem>().QueryAsync(w => itemidList.Contains(w.ItemID));
		if (!resItemDList.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		resOrder = resOrder.Select(s =>
		{
			s.detail.ForEach(e =>
			{
				e.itemname = resItemDList.FirstOrDefault(w => w.ItemID == e.itemid).Name;
			});
			return s;
		}).ToList();
		#endregion

		if (resOrder.Count == 0)
		{
			throw new Exception("ไม่พบข้อมูล");
		}
		return new BaseResponse<GetPurchaseOrderResposeDTO>
		{
			result = true,
			data = resOrder?.FirstOrDefault(),
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
