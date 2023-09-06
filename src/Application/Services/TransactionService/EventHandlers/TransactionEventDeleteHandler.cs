using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTTransactions;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.EventHandlers;
public class TransactionEventDeleteHandler : INotificationHandler<TTTransactionsDeleteEvent>
{
    private readonly ILog4NetLogger _logger;
    public TransactionEventDeleteHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTTransactionsDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"TransactionEventDeleteHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
