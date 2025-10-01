using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMReceiveTemplates;
using CYRetailIMS.Domain.Events.TTPurchaseOrders;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.EventHandlers;
public class ReceiptNoUpdateEventHandler : INotificationHandler<TMReceiptNumberUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ReceiptNoUpdateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMReceiptNumberUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ReceiptNoUpdateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}

