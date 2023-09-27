using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMUsers;
using MediatR;

namespace CYRetailIMS.Application.Services.UserService.EventHandlers;

public class UserCreateEventHandler : INotificationHandler<TMUsersCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public UserCreateEventHandler(ILog4NetLogger logger)
    {
        _logger = logger;
    }
    public Task Handle(TMUsersCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"UserCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}