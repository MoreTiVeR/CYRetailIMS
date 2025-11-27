using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TTTransactionDeletionLogs;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionDeletionLogService.EventHandlers;
public class TranDeletionLogEventCreateHandler : INotificationHandler<TTTransactionDeletionLogsCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public TranDeletionLogEventCreateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTTransactionDeletionLogsCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"TranDeletionLogEventCreateHandler: {notification.GetType().Name}");

        return Task.CompletedTask;
    }
}
