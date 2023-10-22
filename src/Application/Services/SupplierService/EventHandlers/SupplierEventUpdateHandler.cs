using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Domain.Events.TMSupplierContacts;
using MediatR;

namespace CYRetailIMS.Application.Services.SupplierService.EventHandlers;

public class SupplierEventUpdateHandler : INotificationHandler<TMSupplierContactUpdateEvent>
{
    private readonly ILog4NetLogger _logger;
    public SupplierEventUpdateHandler(ILog4NetLogger log4NetLogger)
    {
        _logger = log4NetLogger;
    }
    public Task Handle(TMSupplierContactUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.Info($"SupplierEventUpdateHandler: {notification.GetType().Name}");
        return Task.CompletedTask;
    }
}