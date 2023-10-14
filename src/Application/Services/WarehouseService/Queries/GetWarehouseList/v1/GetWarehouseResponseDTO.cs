using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;

[Serializable]
public class GetWarehouseResponseDTO
{
	public int warehouseid { get; set; }

	public string warehousename { get; set; }

	public string description { get; set; }

	public string createdby { get; set; }

	public DateTime creadeddate { get; set; }

	public bool isactive { get; set; }
}
