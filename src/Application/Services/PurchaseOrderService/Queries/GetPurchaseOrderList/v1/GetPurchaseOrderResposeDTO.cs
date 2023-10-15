using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;

[Serializable]
public class GetPurchaseOrderResposeDTO
{
	public int purchaseorderid { get; set; }

	public string purchaseorderno { get; set; }

	public int purchasetypeid { get; set; }
    public string purchasetypename { get; set; }

    public int supplierid { get; set; }
    public string suppliername { get; set; }

    public int currencyid { get; set; }
    public string currencyname { get; set; }

    public DateTime orderdate { get; set; }

	public DateTime? receiveddate { get; set; }

	public int paymentypeid { get; set; }

    public string paymentypename { get; set; }

    public string remarks { get; set; }

	public decimal amount { get; set; }

	public decimal discount { get; set; }

	public decimal subtotal { get; set; }

	public decimal tax { get; set; }

	public decimal total { get; set; }

	public string createdby { get; set; }

	public DateTime creadeddate { get; set; }

	public bool isactive { get; set; }

	public int approvestatus { get; set; }

	public GetPurchaseOrderShipmentResponseDTO shipment { get; set; }

	public List<GetPurchaseOrderDetailResponseDTO> detail { get; set; }

}
