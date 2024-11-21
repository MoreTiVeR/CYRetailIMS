using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTMoneyTransfers;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.EventHandlers;

public class DeleteMoneyTransferEventHandler : INotificationHandler<TTMoneyTransferDeleteEvent>
{
    private readonly ILog4NetLogger _logger;
    public DeleteMoneyTransferEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTMoneyTransferDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"DeleteMoneyTransferEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}