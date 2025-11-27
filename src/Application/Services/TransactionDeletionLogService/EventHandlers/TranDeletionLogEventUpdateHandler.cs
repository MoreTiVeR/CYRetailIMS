using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TTTransactionDeletionLogs;
using MediatR;

namespace CYRetailIMS.Application.Services.TransactionDeletionLogService.EventHandlers;

public class TranDeletionLogEventUpdateHandler : INotificationHandler<TTTransactionDeletionLogsUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public TranDeletionLogEventUpdateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TTTransactionDeletionLogsUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"TranDeletionLogEventDeleteHandler: {notification.GetType().Name}");

        return Task.CompletedTask;
    }
}
