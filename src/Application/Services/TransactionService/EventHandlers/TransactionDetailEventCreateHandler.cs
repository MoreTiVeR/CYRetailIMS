using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTTransactionDetails;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionService.EventHandlers;
public class TransactionDetailEventCreateHandler : INotificationHandler<TTTransactionDetailCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public TransactionDetailEventCreateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTTransactionDetailCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"TransactionDetailEventCreateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
