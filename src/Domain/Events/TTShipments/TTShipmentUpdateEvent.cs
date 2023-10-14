using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTShipments;

public class TTShipmentUpdateEvent : BaseEvent
{
	public TTShipment Item { get; set; }
	public TTShipmentUpdateEvent(TTShipment shipment)
	{
		Item = shipment;
	}
}
