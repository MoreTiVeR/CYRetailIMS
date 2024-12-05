using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMBranchs;
using CYRetailIMS.Domain.Events.TMItemBrands;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemBrandService.EventHandlers;

public class ItemBrandCreateEventHandler : INotificationHandler<TMItemBrandCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ItemBrandCreateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }

    public Task Handle(TMItemBrandCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ItemBrandCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
