using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Domain.Events.TTPurchaseOrderDetails;
public class TTPurchaseOrderDetailCreateEvent : BaseEvent
{
	public TTPurchaseOrderDetail Item { get; set; }
	public TTPurchaseOrderDetailCreateEvent(TTPurchaseOrderDetail purchaseOrderDetail)
	{
		Item = purchaseOrderDetail;
	}
}
