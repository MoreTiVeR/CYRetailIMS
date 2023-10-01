using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTAdjustItemTransactions;
using MediatR;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.EventHandlers;

public class AdjustItemUpdateEventHandler : INotificationHandler<TTAdjustItemTransactionUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public AdjustItemUpdateEventHandler(ILog4NetLogger logger)
    {
        _logger = logger;
    }

    public Task Handle(TTAdjustItemTransactionUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"AdjustItemCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
