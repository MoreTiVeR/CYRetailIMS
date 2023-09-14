using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMBranchs;
using MediatR;

namespace CYRetailIMS.Application.Services.BranchService.EventHandlers;

public class BranchUpdateEventHandler : INotificationHandler<TMBranchUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public BranchUpdateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMBranchUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"BranchUpdateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
