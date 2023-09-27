using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMEmployees;
using MediatR;

namespace CYRetailIMS.Application.Services.EmployeeService.EventHandlers;

public class EmployeeDeleteEventHandler : INotificationHandler<TMEmployeeDeleteEvent>
{
    private readonly ILog4NetLogger _logger;
    public EmployeeDeleteEventHandler(ILog4NetLogger logger)
    {
        _logger = logger;
    }

    public Task Handle(TMEmployeeDeleteEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"EmployeeDeleteEventHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}
