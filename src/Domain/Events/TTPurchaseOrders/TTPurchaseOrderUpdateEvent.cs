using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTPurchaseOrders;

public class TTPurchaseOrderUpdateEvent : BaseEvent
{
	public TTPurchaseOrder Item { get; set; }
	public TTPurchaseOrderUpdateEvent(TTPurchaseOrder purchaseOrder)
	{
		Item = purchaseOrder;
	}
}
