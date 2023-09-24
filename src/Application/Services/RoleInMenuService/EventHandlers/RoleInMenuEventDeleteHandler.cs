using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMRoleInMenus;
using MediatR;

namespace CYRetailIMS.Application.Services.RoleInMenuService.EventHandlers;
public class RoleInMenuEventDeleteHandler : INotificationHandler<TMRoleInMenuDeleteEvent>
{
    private readonly ILog4NetLogger _logger;
    public RoleInMenuEventDeleteHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMRoleInMenuDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"RoleInMenuEventDeleteHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
