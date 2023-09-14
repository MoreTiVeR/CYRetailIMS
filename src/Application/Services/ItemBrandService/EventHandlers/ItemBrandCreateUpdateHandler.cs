using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMItemBrands;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemBrandService.EventHandlers;

public class ItemBrandCreateUpdateHandler : INotificationHandler<TMItemBrandUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public ItemBrandCreateUpdateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMItemBrandUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"ItemBrandCreateUpdateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}