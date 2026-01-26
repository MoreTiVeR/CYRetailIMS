using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMSubItemTypes;
using CYRetailIMS.Domain.Events.TMUsers;
using MediatR;

namespace CYRetailIMS.Application.Services.SubItemTypeService.EventHandlers;
public class SubItemTypeCreateEventHandler : INotificationHandler<TMSubItemTypeCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public SubItemTypeCreateEventHandler(ILog4NetLogger logger)
    {
        _logger = logger;
    }
    public Task Handle(TMSubItemTypeCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"SubItemTypeCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
