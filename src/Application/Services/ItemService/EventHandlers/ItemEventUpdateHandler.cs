using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMItems;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemService.EventHandlers;
public class ItemEventUpdateHandler : INotificationHandler<TMItemUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ItemEventUpdateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }

    public Task Handle(TMItemUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ItemEventUpdateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
