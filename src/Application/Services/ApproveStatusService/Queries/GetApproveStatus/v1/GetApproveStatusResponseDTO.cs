using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ApproveStatusService.Queries.GetApproveStatus.v1;
[Serializable]
public class GetApproveStatusResponseDTO
{
	public int approvestatusid { get; set; }

	public string approvestatusname_th { get; set; }

	public string approvestatusname_en { get; set; }

	public string description { get; set; }

	public string createdby { get; set; }

	public DateTime creadeddate { get; set; }

	public bool isactive { get; set; }
}
