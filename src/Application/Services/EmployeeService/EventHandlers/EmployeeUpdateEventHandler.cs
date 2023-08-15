using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMEmployees;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CYRetailIMS.Application.Services.EmployeeService.EventHandlers;
public class EmployeeUpdateEventHandler : INotificationHandler<TMEmployeeUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public EmployeeUpdateEventHandler(ILog4NetLogger logger)
    {
        _logger = logger;
    }

    public Task Handle(TMEmployeeUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"EmployeeUpdateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
