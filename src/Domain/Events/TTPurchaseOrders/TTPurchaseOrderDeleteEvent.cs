using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTPurchaseOrders;

public class TTPurchaseOrderDeleteEvent : BaseEvent
{
	public TTPurchaseOrder Item { get; set; }
	public TTPurchaseOrderDeleteEvent(TTPurchaseOrder purchaseOrder)
	{
		Item = purchaseOrder;
	}
}
