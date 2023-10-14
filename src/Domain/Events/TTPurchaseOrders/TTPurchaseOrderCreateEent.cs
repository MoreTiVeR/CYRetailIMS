using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTPurchaseOrders;
public class TTPurchaseOrderCreateEent : BaseEvent
{
	public TTPurchaseOrder Item { get; set; }
	public TTPurchaseOrderCreateEent(TTPurchaseOrder purchaseOrder)
	{
		Item = purchaseOrder;
	}
}
