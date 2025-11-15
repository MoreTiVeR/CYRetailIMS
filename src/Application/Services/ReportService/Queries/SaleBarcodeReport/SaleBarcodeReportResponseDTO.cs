using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport.v1;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleBarcodeReport;
public class SaleBarcodeReportResponseDTO
{
    public int totalrow { get; set; }
    public List<SaleBarcodeReportResponseDetailDTO> data { get; set; }
}
