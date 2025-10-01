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
public class TTReceiptUpdateEventHandler : INotificationHandler<TTReceiptUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public TTReceiptUpdateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTReceiptUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"TTReceiptUpdateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}

