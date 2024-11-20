using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ExcelAPI;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.ExcelService.Queries.GenerateStockTransferExcelReport.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ExcelAPI;
public class ExcelAPI :HttpClientService, IExcelAPI
{
    public ExcelAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GenerateStockTransferExcelReportResponseDTO>> GenerateInventoryTransferExcelAsync(GenerateStockTransferExcelReportQuery excelReportQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<GenerateStockTransferExcelReportResponseDTO,
            GenerateStockTransferExcelReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/excel/v1/generate"), excelReportQuery);
    }
}
