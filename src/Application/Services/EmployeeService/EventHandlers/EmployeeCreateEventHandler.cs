using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMEmployees;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CYRetailIMS.Application.Services.EmployeeService.EventHandlers;
public class EmployeeCreateEventHandler : INotificationHandler<TMEmployeeCreateEvent>
{
    private readonly ILog4NetLogger _logger;
    public EmployeeCreateEventHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }

    public Task Handle(TMEmployeeCreateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"EmployeeCreateEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
