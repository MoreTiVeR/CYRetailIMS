using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.EventHandlers;
public class ItemInBranchCreateHandler : INotificationHandler<TMItemInBranchCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ItemInBranchCreateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMItemInBranchCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ItemInBranchCreateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}