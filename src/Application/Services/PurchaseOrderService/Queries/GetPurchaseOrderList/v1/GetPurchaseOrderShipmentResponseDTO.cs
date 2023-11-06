using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;

[Serializable]
public class GetPurchaseOrderShipmentResponseDTO
{
	public int shipmentid { get; set; }

	public int shipmenttypeid { get; set; }

    public string shipmenttypename { get; set; }

    public int? warehouseid { get; set; }

    public string warehousename { get; set; }

	public string shipmentname { get; set; }

	public DateTime? shipmentdate { get; set; }

	public string trackingno { get; set; }

	public string createdby { get; set; }

	public DateTime createddate { get; set; }

	public bool isactive { get; set; }
}
