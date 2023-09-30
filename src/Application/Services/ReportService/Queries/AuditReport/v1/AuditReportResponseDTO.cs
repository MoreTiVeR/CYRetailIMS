using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;

[Serializable]
public class AuditReportResponseDTO
{
	#region Transaction
	//public int transactionid { get; set; }
	public DateTime transactiondate { get; set; }
	public decimal totalamount { get; set; }
	public decimal amounttransfer { get; set; }
	public decimal amountdeposit { get; set; }
	public decimal amountcash { get; set; }
	public decimal depositfee { get; set; }
	//public string createdby { get; set; }
	//public string createdbystaff { get; set; }
	#endregion

	#region Audit
	//public int? auditid { get; set; }
	public decimal? totalauditamount { get; set; }
	public string? auditdescription { get; set; }
	#endregion
}
