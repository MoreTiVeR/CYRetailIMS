using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;
public class SaleReportGroupByBranchResposneDTO
{
    public int totalrow { get; set; }
    public List<SaleReportGroupByBranchDetailDTO> transactiondata { get; set; }
}
