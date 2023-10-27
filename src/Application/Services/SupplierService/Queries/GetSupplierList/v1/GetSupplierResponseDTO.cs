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

    #region Contact
    public int suppliercontacttypeid { get; init; }
    public string suppliercontacttypename { get; init; }
    public string contactaccountname { get; init; }
    public string contactperson { get; init; }
    public string mobileno { get; init; }
    public string contactdesctiption { get; init; }
    #endregion

    public string createdby { get; set; }

    public DateTime createddate { get; set; }

    public bool isactive { get; set; }
}
