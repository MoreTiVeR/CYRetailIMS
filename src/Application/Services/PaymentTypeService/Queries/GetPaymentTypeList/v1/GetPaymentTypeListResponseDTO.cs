using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;

[Serializable]
public class GetPaymentTypeListResponseDTO
{
	public int paymentypeid { get; set; }

	public string paymentypecode { get; set; }

	public string paymentypename { get; set; }

	public string description { get; set; }

	public string createdby { get; set; }

	public DateTime creadeddate { get; set; }

	public bool isactive { get; set; }
}
