using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;

[Serializable]
public class GetPurchaseTypeResponseDTO
{
	public int purchasetypeid { get; set; }

	public string purchasetypecode { get; set; }

	public string purchasetypename { get; set; }

	public string description { get; set; }

	public string createdby { get; set; }

	public DateTime createddate { get; set; }

	public bool isactive { get; set; }

}
