using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMReceiveTemplates;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.EventHandlers;
public class ReceiveTempUpdateEventHandler : INotificationHandler<TMReceiveTemplateUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ReceiveTempUpdateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMReceiveTemplateUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ReceiveTempUpdateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
