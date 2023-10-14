using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;

[Serializable]
public class GetShipmentTypeResponseDTO
{
	public int shipmenttypeid { get; set; }

	public string shipmenttypename { get; set; }

	public string description { get; set; }

	public string createdby { get; set; }

	public DateTime creadeddate { get; set; }

	public bool isactive { get; set; }
}
