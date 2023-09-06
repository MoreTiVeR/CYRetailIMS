using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTTransactions;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.EventHandlers;
public class TransactionEventCreateHandler : INotificationHandler<TTTransactionsCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public TransactionEventCreateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTTransactionsCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"TransactionEventCreateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
