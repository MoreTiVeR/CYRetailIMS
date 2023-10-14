using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
public class CreatePurchaseOrderMappingProfile : Profile
{

	public CreatePurchaseOrderMappingProfile()
	{
		CreateMap<CreatePurchaseOrderCommand, TTPurchaseOrder>()
			.ForMember(w => w.PurchaseTypeID, f => f.MapFrom(w => w.purchasetypeid))
			.ForMember(w => w.SupplierID, f => f.MapFrom(w => w.supplierid))
			.ForMember(w => w.CurrencyID, f => f.MapFrom(w => w.currencyid))
			.ForMember(w => w.OrderDate, f => f.MapFrom(w => w.orderdate))
			.ForMember(w => w.ReceivedDate, f => f.MapFrom(w => w.receiveddate))
			.ForMember(w => w.PaymenTypeID, f => f.MapFrom(w => w.purchasetypeid))
			.ForMember(w => w.Remarks, f => f.MapFrom(w => w.remarks))
			.ForMember(w => w.Amount, f => f.MapFrom(w => w.amount))
			.ForMember(w => w.Discount, f => f.MapFrom(w => w.discount))
			.ForMember(w => w.SubTotal, f => f.MapFrom(w => w.subtotal))
			.ForMember(w => w.Tax, f => f.MapFrom(w => w.tax))
			.ForMember(w => w.Total, f => f.MapFrom(w => w.total))
			.ForMember(w => w.CreatedBy, f => f.MapFrom(w => w.createdby))
			//.ForMember(w => w.TTShipments, f => f.MapFrom(w => w.shipment))
			.ForMember(w => w.TTPurchaseOrderDetails, f => f.MapFrom(w => w.detail));

		//Detail
		CreateMap<CreatePurchaseOrderDetailCommand, TTPurchaseOrderDetail>()
			.ForMember(w => w.ItemID, f => f.MapFrom(w => w.itemid))
			.ForMember(w => w.Description, f => f.MapFrom(w => w.description))
			.ForMember(w => w.Quantity, f => f.MapFrom(w => w.qty))
			.ForMember(w => w.Price, f => f.MapFrom(w => w.price))
			.ForMember(w => w.DiscountPercentage, f => f.MapFrom(w => w.discountpercentage))
			.ForMember(w => w.DiscountAmount, f => f.MapFrom(w => w.discountamount))
			.ForMember(w => w.SubTotal, f => f.MapFrom(w => w.subtotal))
			.ForMember(w => w.TaxPercentage, f => f.MapFrom(w => w.taxpercentage))
			.ForMember(w => w.TaxAmount, f => f.MapFrom(w => w.taxamount))
			.ForMember(w => w.Total, f => f.MapFrom(w => w.total));

		CreateMap<CreateShipmentCommand, TTShipment>()
			.ForMember(w => w.ShipmentTypeID, f => f.MapFrom(w => w.shipmenttypeid))
			.ForMember(w => w.WarehouseID, f => f.MapFrom(w => w.warehouseid))
			.ForMember(w => w.ShipmentName, f => f.MapFrom(w => w.shipmentname))
			.ForMember(w => w.ShipmentDate, f => f.MapFrom(w => w.shipmentdate))
			.ForMember(w => w.TrackingNo, f => f.MapFrom(w => w.trackingno));
	}
}
