using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.CountStockAPI;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.CountStockAPI;
public class CountStockAPI : HttpClientService, ICountStockAPI
{
    public CountStockAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<List<InquiryCountStockResponseDTO>>> GetCountStockListAsync(InquiryCountStocksQuery inquiryObj)
    {
        return await _httpClientRequest.HttpRequestToObject<List<InquiryCountStockResponseDTO>, InquiryCountStocksQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/inquiry"), inquiryObj);
    }
}
