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
		IEnumerable<TTPurchaseOrder> resPurchaseEntity = await _unitOfWork.Repository<TTPurchaseOrder>().FindWithInclude(w =>
		w.PurchaseOrderID == request.purchaseorderid, i => i.Include(w => w.TTPurchaseOrderDetails), ii => ii.Include(w => w.TTShipments));
		if (!resPurchaseEntity.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}

		resPurchaseEntity.ToList().ForEach(e =>
		{
			e.SetCreatedBy(request.updatedby);
			e.SetUpdatedDate(request.updateddate);
			e.IsActive = request.isactive;
			e.AddDomainEvent(new TTPurchaseOrderUpdateEvent(e));
			e.TTPurchaseOrderDetails.ToList().ForEach(detail =>
			{
				detail.SetCreatedBy(request.updatedby);
				detail.SetUpdatedDate(request.updateddate);
				detail.IsActive = request.isactive;
				detail.AddDomainEvent(new TTPurchaseOrderDetailUpdateEvent(detail));
			});
			e.TTShipments.ToList().ForEach(shipment =>
			{
				shipment.SetCreatedBy(request.updatedby);
				shipment.SetUpdatedDate(request.updateddate);
				shipment.IsActive = request.isactive;
				shipment.AddDomainEvent(new TTShipmentUpdateEvent(shipment));
			});
		});

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
