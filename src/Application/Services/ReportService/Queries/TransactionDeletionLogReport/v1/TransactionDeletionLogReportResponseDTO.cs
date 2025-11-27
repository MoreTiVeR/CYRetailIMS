using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;

namespace CYRetailIMS.Application.Services.ReportService.Queries.TransactionDeletionLogReport.v1;
public class TransactionDeletionLogReportResponseDTO
{
    public int totalrow { get; set; }
    public List<TransactionDeletionLogReportDetailDTO> transactiondata { get; set; }
}
