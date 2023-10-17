using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTPurchaseOrderDetails;
using CYRetailIMS.Domain.Events.TTPurchaseOrders;
using CYRetailIMS.Domain.Events.TTShipments;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.UpdatePurchaseOrder.v1;
public class UpdatePurchaseOrderHandler : BaseService, IRequestHandler<UpdatePurchaseOrderCommand, BaseResponse<CommandResponse>>
{
	public UpdatePurchaseOrderHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	/// <summary>
	/// Approve PurchaseOrder
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public async Task<BaseResponse<CommandResponse>> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
	{
		if(request.detail == null || request.detail?.Count == 0)
		{
            throw new Exception("กรุณาเพิ่มสินค้าสั่งซื้อก่อนทำรายการ");
        }

		IEnumerable<TTPurchaseOrder> resPurchaseEntity = await _unitOfWork.Repository<TTPurchaseOrder>().FindWithInclude(w =>
		w.PurchaseOrderID == request.purchaseorderid, i => i.Include(w => w.TTPurchaseOrderDetails), ii => ii.Include(w => w.TTShipments));
		if (!resPurchaseEntity.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}

		var purchaseEnt = resPurchaseEntity.FirstOrDefault();
        if (purchaseEnt.ApproveStatus == (int)EnumModel.ApproveStatus.Approve && request.approvestatus != (int)EnumModel.ApproveStatus.Approve)
		{
            throw new Exception("ไม่สามารถเปลี่ยนสถานะการขนส่งได้ เนื่องจากรับสินค้านี้ไปแล้ว");
        }

		//Update PurchaseOrder
        resPurchaseEntity.ToList().ForEach(e =>
		{
			e.SetCreatedBy(request.updatedby);
			e.SetUpdatedDate(request.updateddate);
            e.ApproveStatus = request.approvestatus;
            e.AddDomainEvent(new TTPurchaseOrderUpdateEvent(e));
			e.TTPurchaseOrderDetails.ToList().ForEach(detail =>
			{
				var updateEnt = request.detail.FirstOrDefault(w => w.itemid == detail.ItemID);
				if(updateEnt == null)
				{
                    detail.SetCreatedBy(request.updatedby);
                    detail.SetUpdatedDate(request.updateddate);
					detail.DeActiveStatus();
                    detail.AddDomainEvent(new TTPurchaseOrderDetailUpdateEvent(detail));
                }
			});
			e.TTShipments.ToList().ForEach(shipment =>
			{
				shipment.SetCreatedBy(request.updatedby);
				shipment.SetUpdatedDate(request.updateddate);
				if (!string.IsNullOrEmpty(request.trackingno))
				{
					shipment.TrackingNo = request.trackingno;
                }
				shipment.AddDomainEvent(new TTShipmentUpdateEvent(shipment));
			});
		});

		//Update TMItem
		//Coding

		await _unitOfWork.SaveChangesAsync();
		return new BaseResponse<CommandResponse>
		{
			result = true,
			data = new CommandResponse { result = true },
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}
}
