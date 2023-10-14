using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;

[Serializable]
public record CreateShipmentCommand
{
	public int shipmenttypeid { get; set; }

	public int? warehouseid { get; set; }

	public string shipmentname { get; set; }

	public DateTime? shipmentdate { get; set; }

	public string trackingno { get; set; }

	//public string createdby { get; set; }

	//public DateTime creadeddate { get; set; }
}
