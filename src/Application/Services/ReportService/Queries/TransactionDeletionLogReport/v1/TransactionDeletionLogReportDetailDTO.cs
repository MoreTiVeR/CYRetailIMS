using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.TransactionDeletionLogReport.v1;
public class TransactionDeletionLogReportDetailDTO
{
    public int deltransactionlogid { get; set; }
    public int transactionid { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }
    public int transactiontypeid { get; set; }
    public string transactiontypename { get; set; }
    public string transactiontypedesc { get; set; }
    public decimal totalamount { get; set; }
    public string reason { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }
    public string? createdbystaff { get; set; }


    // (Optional) update metadata — for admin audit
    public string? updatedby { get; set; }
    public DateTime? updateddate { get; set; }

    //// Extra fields from TTTransaction (for report display)
    //public DateTime? originaltransactiondate { get; set; }
    //public decimal? totalamount { get; set; }
    //public decimal? amountcash { get; set; }
    //public decimal? amounttransfer { get; set; }
    //public decimal? amountdeposit { get; set; }


    //// Optional — transaction type name joined from TMTransactionType
    //public string? transactiontypename { get; set; }
}
