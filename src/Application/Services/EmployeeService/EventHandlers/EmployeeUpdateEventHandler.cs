using CYRetailIMS.Domain.Events.TMEmployees;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CYRetailIMS.Application.Services.EmployeeService.EventHandlers;
public class EmployeeUpdateEventHandler : INotificationHandler<TMEmployeeUpdateEvent>
{
    private readonly ILogger<EmployeeUpdateEventHandler> _logger;
    public EmployeeUpdateEventHandler(ILogger<EmployeeUpdateEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TMEmployeeUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"EmployeeUpdateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
