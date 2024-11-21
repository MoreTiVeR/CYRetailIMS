using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CYRetailIMS.Application.Services.ExcelService.Queries.GenerateStockTransferExcelReport.v1;
public class GenerateStockTransferExcelReportResponseDTO
{
    public string filename { get; set; }
    public byte[] filebyte { get; set; }
}
