using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;

[Serializable]
public class GetPurchaseOrderDetailResponseDTO
{
	public int purchaseorderdetailid { get; set; }

	public int itemid { get; set; }
    public string itemname { get; set; }

    public string description { get; set; }

	public int quantity { get; set; }

	public decimal price { get; set; }

	public decimal amount { get; set; }

	public decimal discountpercentage { get; set; }

	public decimal discountamount { get; set; }

	public decimal subtotal { get; set; }

	public decimal taxpercentage { get; set; }

	public decimal taxamount { get; set; }

	public decimal total { get; set; }

	public bool isactive { get; set; }
}
