using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTPurchaseOrderDetails;
using CYRetailIMS.Domain.Events.TTPurchaseOrders;
using CYRetailIMS.Domain.Events.TTShipments;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
public class CreatePurchaseOrderHandler : BaseService, IRequestHandler<CreatePurchaseOrderCommand, BaseResponse<CommandResponse>>
{
    public CreatePurchaseOrderHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
		TTPurchaseOrder tmPurchaseOrderEnt = _mapper.Map<TTPurchaseOrder>(request);

		tmPurchaseOrderEnt.TTPurchaseOrderDetails.ToList().ForEach(w =>
		{
			w.SetCreatedBy(request.createdby);
			w.SetCreatedDate(request.createddate);
			w.AddDomainEvent(new TTPurchaseOrderDetailCreateEvent(w));
		});

		TTShipment shipment = _mapper.Map<TTShipment>(request.shipment);
		shipment.SetCreatedBy(request.createdby);
		shipment.SetCreatedDate(request.createddate);
		shipment.AddDomainEvent(new TTShipmentCreateEvent(shipment));
		tmPurchaseOrderEnt.TTShipments.Add(shipment);

		tmPurchaseOrderEnt.PurchaseOrderNo = PurchaseOrderNoGenerator.GeneratePO();
		tmPurchaseOrderEnt.SetCreatedDate(request.createddate);
		tmPurchaseOrderEnt.SetCreatedBy(request.createdby);
		tmPurchaseOrderEnt.AddDomainEvent(new TTPurchaseOrderCreateEent(tmPurchaseOrderEnt));

		await _unitOfWork.Repository<TTPurchaseOrder>().AddAsync(tmPurchaseOrderEnt);
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


	private TTShipment MappingShipmentEntity(CreatePurchaseOrderCommand request)
	{
		TTShipment shipment = new TTShipment
		{
			ShipmentTypeID = request.shipment.shipmenttypeid,
			WarehouseID = request.shipment.warehouseid,
			ShipmentName = request.shipment.shipmentname,
			ShipmentDate = request.shipment.shipmentdate,
			TrackingNo = request.shipment.trackingno,
			CreadedDate = request.createddate,
			CreatedBy = request.createdby
		};
		if (request.shipment.shipmentdate.HasValue)
		{
			shipment.ShipmentDate = request.shipment.shipmentdate.Value;
		}
		return shipment;
	}

}
