using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMBranchs;
using CYRetailIMS.Domain.Events.TTMoneyTransfers;
using MediatR;

namespace CYRetailIMS.Application.Services.MoneyTransferService.EventHandlers;

public class CreateMoneyTransferEventHandler : INotificationHandler<TTMoneyTransferCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public CreateMoneyTransferEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTMoneyTransferCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"CreateMoneyTransferEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}