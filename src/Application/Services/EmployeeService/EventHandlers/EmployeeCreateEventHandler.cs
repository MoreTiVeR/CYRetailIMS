using CYRetailIMS.Domain.Events.TMEmployees;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CYRetailIMS.Application.Services.EmployeeService.EventHandlers;
public class EmployeeCreateEventHandler : INotificationHandler<TMEmployeeCreateEvent>
{
    private readonly ILogger<EmployeeCreateEventHandler> _logger;
    public EmployeeCreateEventHandler(ILogger<EmployeeCreateEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TMEmployeeCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"EmployeeCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
