using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTPurchaseOrderDetails;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.EventHandlers;

public class PurchaseOrderDetailDeleteEventHandler : INotificationHandler<TTPurchaseOrderDetailDeleteEvent>
{
    private readonly ILog4NetLogger _logger;
    public PurchaseOrderDetailDeleteEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTPurchaseOrderDetailDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"PurchaseOrderDetailDeleteEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
