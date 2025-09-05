using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMReceiveTemplates;
using MediatR;

namespace CYRetailIMS.Application.Services.ReceiveTempService.EventHandlers;

public class ReceiveTempDeleteEventHandler : INotificationHandler<TMReceiveTemplateDeleteEvent>
{
    private readonly ILog4NetLogger _logger;
    public ReceiveTempDeleteEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMReceiveTemplateDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ReceiveTempDeleteEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
