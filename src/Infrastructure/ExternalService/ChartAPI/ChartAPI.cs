using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ChartAPI;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryBarchart.v1;
using CYRetailIMS.Application.Services.ChartService.Queries.GetMontlySaleSummaryByYear.v1;
using CYRetailIMS.Application.Services.ChartService.Queries.GetSellingTransactionByMonth.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ChartAPI;
public class ChartAPI : HttpClientService, IChartAPI
{
    public ChartAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>> GetMontlySameSummaryAsync(GetMontlySaleSummaryBarchartQuery summaryBarchartQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetMontlySaleSummaryBarchartResponseDTO>, GetMontlySaleSummaryBarchartQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/chart/v1/montlysale"), summaryBarchartQuery);
    }

    public async Task<BaseResponse<List<GetMontlySaleSummaryBarchartResponseDTO>>> GetMontlySameSummaryV2Async(GetMontlySaleSummaryBarchartQuery summaryBarchartQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetMontlySaleSummaryBarchartResponseDTO>, GetMontlySaleSummaryBarchartQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/chart/v2/montlysale"), summaryBarchartQuery);
    }

    public async Task<BaseResponse<List<GetMontlySaleSummaryByYearResponseDTO>>> GetMontlySaleSummaryByYearAsync(GetMontlySaleSummaryByYearQuery saleSummaryByYearQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetMontlySaleSummaryByYearResponseDTO>, GetMontlySaleSummaryByYearQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/chart/v1/yearsale"), saleSummaryByYearQuery);
    }

    public async Task<BaseResponse<List<GetSellingTransactionByMonthResponseDTO>>> GetSellingItemPercnetageAsync(GetTransactionByMonthQuery transactionByMonthQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetSellingTransactionByMonthResponseDTO>, GetTransactionByMonthQuery>(HttpMethod.Post, 
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/chart/v1/sellingitempercentage"), transactionByMonthQuery);
    }
}
