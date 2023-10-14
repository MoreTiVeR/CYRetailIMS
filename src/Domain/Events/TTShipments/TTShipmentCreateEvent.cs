using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTShipments;
public class TTShipmentCreateEvent : BaseEvent
{
    public TTShipment Item { get; set; }
    public TTShipmentCreateEvent(TTShipment shipment)
	{
		Item = shipment;
	}
}
