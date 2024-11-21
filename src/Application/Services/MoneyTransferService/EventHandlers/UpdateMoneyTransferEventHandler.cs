using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTMoneyTransfers;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.EventHandlers;

public class UpdateMoneyTransferEventHandler : INotificationHandler<TTMoneyTransferUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public UpdateMoneyTransferEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTMoneyTransferUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"UpdateMoneyTransferEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}