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

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.DeletePurchaseOrder.v1;
public class DeletePurchaseOrderHandler : BaseService, IRequestHandler<DeletePurchaseOrderCommand, BaseResponse<CommandResponse>>
{
    public DeletePurchaseOrderHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
		IEnumerable<TTPurchaseOrder> resPurchaseEntity = await _unitOfWork.Repository<TTPurchaseOrder>().FindWithInclude(w =>
		w.PurchaseOrderID == request.purchaseorderid, i => i.Include(w => w.TTPurchaseOrderDetails), ii => ii.Include(w => w.TTShipments));
		if (!resPurchaseEntity.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}

		resPurchaseEntity.ToList().ForEach(e =>
		{
			e.SetCreatedBy(request.deletedby);
			e.SetUpdatedDate(request.deleteddate);
			e.DeActiveStatus();
			e.AddDomainEvent(new TTPurchaseOrderUpdateEvent(e));
			e.TTPurchaseOrderDetails.ToList().ForEach(detail =>
			{
				detail.DeActiveStatus();
				detail.SetCreatedBy(request.deletedby);
				detail.SetUpdatedDate(request.deleteddate);
				detail.AddDomainEvent(new TTPurchaseOrderDetailDeleteEvent(detail));
			});
			e.TTShipments.ToList().ForEach(shipment =>
			{
				shipment.DeActiveStatus();
				shipment.SetCreatedBy(request.deletedby);
				shipment.SetUpdatedDate(request.deleteddate);
				shipment.AddDomainEvent(new TTShipmentDeleteEvent(shipment));
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
