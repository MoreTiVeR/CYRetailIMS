using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;

[Serializable]
public class GetSupplierContactTypeResposeDTO
{
	public int suppliercontacttypeid { get; set; }

	public string suppliercontacttypename { get; set; }

	public string description { get; set; }

	public string createdby { get; set; }

	public DateTime creadeddate { get; set; }

	public bool isactive { get; set; }

}
