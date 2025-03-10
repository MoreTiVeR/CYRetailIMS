using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;

[Serializable]
public class SaleReportResponseDTO
{
    public int totalrow { get; set; }
    public List<SaleReportResponseDetailDTO> transactiondata { get; set; }

}
