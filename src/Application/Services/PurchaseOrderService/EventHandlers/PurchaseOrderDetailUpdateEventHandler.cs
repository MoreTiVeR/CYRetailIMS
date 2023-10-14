using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTPurchaseOrderDetails;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.EventHandlers;

public class PurchaseOrderDetailUpdateEventHandler : INotificationHandler<TTPurchaseOrderDetailUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public PurchaseOrderDetailUpdateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }

    public Task Handle(TTPurchaseOrderDetailUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"PurchaseOrderDetailUpdateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}