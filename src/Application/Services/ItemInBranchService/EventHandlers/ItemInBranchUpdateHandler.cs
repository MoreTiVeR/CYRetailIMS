using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMItemInBranchs;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemInBranchService.EventHandlers;
public class ItemInBranchUpdateHandler : INotificationHandler<TMItemInBranchUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ItemInBranchUpdateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMItemInBranchUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ItemInBranchUpdateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
