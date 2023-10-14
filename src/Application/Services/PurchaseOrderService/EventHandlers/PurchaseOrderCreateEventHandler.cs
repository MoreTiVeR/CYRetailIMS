using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTPurchaseOrders;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.EventHandlers;
public class PurchaseOrderCreateEventHandler : INotificationHandler<TTPurchaseOrderCreateEent>
{
    private readonly ILog4NetLogger _logger;
    public PurchaseOrderCreateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTPurchaseOrderCreateEent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"PurchaseOrderCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
