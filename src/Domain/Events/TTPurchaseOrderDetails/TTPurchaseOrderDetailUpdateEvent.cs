using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTPurchaseOrderDetails;

public class TTPurchaseOrderDetailUpdateEvent : BaseEvent
{
	public TTPurchaseOrderDetail Item { get; set; }
	public TTPurchaseOrderDetailUpdateEvent(TTPurchaseOrderDetail purchaseOrderDetail)
	{
		Item = purchaseOrderDetail;
	}
}
