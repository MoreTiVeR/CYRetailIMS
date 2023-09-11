using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.EventHandlers;
public class ItemTransferEventCreateHandler : INotificationHandler<TTItemTransferCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ItemTransferEventCreateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTItemTransferCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ItemTransferEventCreateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
