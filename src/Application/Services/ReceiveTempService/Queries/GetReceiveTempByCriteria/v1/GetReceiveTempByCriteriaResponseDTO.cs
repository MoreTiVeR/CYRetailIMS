using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReportGroupByBranch.v1;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByCriteria.v1;
public class GetReceiveTempByCriteriaResponseDTO
{
    public int totalrow { get; set; }
    public List<GetReceiveTempResponseDTO> receipttemplates { get; set; }
}
