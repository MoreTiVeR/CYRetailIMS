using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.Report;
using CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ReportAPI;
public class ReportAPI : HttpClientService, IReportAPI
{
    public ReportAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateAuditTransactionReportAsync(CreateAuditReportCommand createAuditReportCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            CreateAuditReportCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/createaudittransaction"), createAuditReportCommand);
    }

    public async Task<BaseResponse<List<SaleReportResponseDTO>>> GetSaleReportAsync(SaleReportQuery saleReportQuery)
    {
		return await _httpClientRequest.HttpRequestToObject<List<SaleReportResponseDTO>, 
            SaleReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salereport"), saleReportQuery);
	}

    public async Task<BaseResponse<List<SaleSummaryReportResponseDTO>>> GetSaleSummaryReportAsync(SaleSummaryReportQuery saleSummaryReportQuery)
    {
		return await _httpClientRequest.HttpRequestToObject<List<SaleSummaryReportResponseDTO>,
			SaleSummaryReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salesummaryreport"), saleSummaryReportQuery);
	}

    public async Task<BaseResponse<List<AuditReportResponseDTO>>> GetAuditReportAsync(AuditReportQuery auditReportQuery)
    {
		return await _httpClientRequest.HttpRequestToObject<List<AuditReportResponseDTO>,
			AuditReportQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/auditreport"), auditReportQuery);
	}

    public async Task<BaseResponse<SaleSummaryReportResponseDTO>> GetSaleSummaryReportByTransIDAsync(int transactionid)
    {
        return await _httpClientRequest.HttpRequestToObject<SaleSummaryReportResponseDTO,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/report/v1/salesummaryreportbytransid/{transactionid}"), null);
    }

    
}
