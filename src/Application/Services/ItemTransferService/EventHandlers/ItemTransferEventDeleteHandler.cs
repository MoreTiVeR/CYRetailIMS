using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTItemTransfers;
using CYRetailIMS.Domain.Events.TTTransactions;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemTransferService.EventHandlers;
public class ItemTransferEventDeleteHandler : INotificationHandler<TTItemTransferDeleteEvent>
{
    private readonly ILog4NetLogger _logger;
    public ItemTransferEventDeleteHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTItemTransferDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ItemTransferEventDeleteHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
