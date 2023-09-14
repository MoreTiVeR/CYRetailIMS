using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMBranchs;
using MediatR;

namespace CYRetailIMS.Application.Services.BranchService.EventHandlers;
public class BranchCreateEventHandler : INotificationHandler<TMBranchCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public BranchCreateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMBranchCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"BranchCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
