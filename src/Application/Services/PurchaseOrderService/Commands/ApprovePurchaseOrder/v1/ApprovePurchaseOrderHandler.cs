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

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.ApprovePurchaseOrder.v1;
public class ApprovePurchaseOrderHandler : BaseService, IRequestHandler<ApprovePurchaseOrderCommand, BaseResponse<CommandResponse>>
{
	public ApprovePurchaseOrderHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
	{
	}

	/// <summary>
	/// Approve PurchaseOrder
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public async Task<BaseResponse<CommandResponse>> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
	{
		IEnumerable<TTPurchaseOrder> resPurchaseEntity = await _unitOfWork.Repository<TTPurchaseOrder>().QueryAsync(w => 
		w.PurchaseOrderID == request.purchaseorderid);
		if (!resPurchaseEntity.Any())
		{
			throw new Exception("ไม่พบข้อมูล");
		}

		resPurchaseEntity.ToList().ForEach(e =>
		{
			e.SetCreatedBy(request.approvedby);
			e.SetUpdatedDate(request.approveddate);
			e.ApproveStatus = request.approvestatus;
			e.AddDomainEvent(new TTPurchaseOrderUpdateEvent(e));
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
