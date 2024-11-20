using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ExcelService.Queries.GenerateStockTransferExcelReport.v1;

namespace CYRetailIMS.Application.ExternalService.ExcelAPI;
public interface IExcelAPI
{
    Task<BaseResponse<GenerateStockTransferExcelReportResponseDTO>> GenerateInventoryTransferExcelAsync(GenerateStockTransferExcelReportQuery excelReportQuery);
}
