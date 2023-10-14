using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;

[Serializable]
public class GetSupplierResponseDTO
{
	public int supplierid { get; set; }

	public string suppliername_th { get; set; }

	public string suppliername_en { get; set; }

	public int suppliertypeid { get; set; }

	public string suppliertypename { get; set; }

	public string description { get; set; }

	public string createdby { get; set; }

	public DateTime creadeddate { get; set; }

	public bool isactive { get; set; }
}
