using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.EventHandlers;
public class ItemTransferEventUpdateHandler : INotificationHandler<TTItemTransferUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ItemTransferEventUpdateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTItemTransferUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ItemTransferEventUpdateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
