using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;

[Serializable]
public class GetItemTransferStatusResponseDTO
{
	public int transferstatusid { get; set; }

	public string transferstatusname_th { get; set; }

	public string transferstatusname_en { get; set; }

	public string description { get; set; }

	public string createdby { get; set; }

	public DateTime creadeddate { get; set; }

	public bool isactive { get; set; }

}
