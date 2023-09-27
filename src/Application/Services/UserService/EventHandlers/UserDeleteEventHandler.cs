using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMUsers;
using MediatR;

namespace CYRetailIMS.Application.Services.UserService.EventHandlers;

public class UserDeleteEventHandler : INotificationHandler<TMUsersDeleteEvent>
{
    private readonly ILog4NetLogger _logger;
    public UserDeleteEventHandler(ILog4NetLogger logger)
    {
        _logger = logger;
    }
    public Task Handle(TMUsersDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"UserDeleteEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
