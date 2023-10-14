using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTPurchaseOrderDetails;
using CYRetailIMS.Domain.Events.TTPurchaseOrders;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.EventHandlers;
public class PurchaseOrderDetailCreateEventHandler : INotificationHandler<TTPurchaseOrderDetailCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public PurchaseOrderDetailCreateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTPurchaseOrderDetailCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"PurchaseOrderDetailCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
