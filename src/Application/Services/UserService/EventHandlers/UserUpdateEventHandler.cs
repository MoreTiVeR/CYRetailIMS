using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMUsers;
using MediatR;

namespace CYRetailIMS.Application.Services.UserService.EventHandlers;
public class UserUpdateEventHandler : INotificationHandler<TMUsersUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public UserUpdateEventHandler(ILog4NetLogger logger)
    {
        _logger = logger;
    }
    public Task Handle(TMUsersUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"UserUpdateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
