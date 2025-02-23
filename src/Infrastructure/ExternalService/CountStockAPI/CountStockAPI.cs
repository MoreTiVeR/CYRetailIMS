using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.CountStockAPI;
using CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.CountStockAPI;
public class CountStockAPI : HttpClientService, ICountStockAPI
{
    public CountStockAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateCountStockListAsync(CreateCountStockCommand createCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateCountStockCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/create"), createCommand);
    }

    public async Task<BaseResponse<List<InquiryCountStockResponseDTO>>> GetCountStockListAsync(InquiryCountStocksQuery inquiryObj)
    {
        return await _httpClientRequest.HttpRequestToObject<List<InquiryCountStockResponseDTO>, InquiryCountStocksQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/inquiry"), inquiryObj);
    }

    public async Task<BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>> InquiryCountStockByBranchIDAsync(InquiryCountStockByBranchIDQuery inquiryObj)
    {
        return await _httpClientRequest.HttpRequestToObject<List<InquiryCountStockByBranchIDResponseDTO>, InquiryCountStockByBranchIDQuery>(HttpMethod.Post,
           new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/inquiry-countstock-bybranch"), inquiryObj);
    }

    public async Task<BaseResponse<InquiryCountStockByIDResponseDTO>> InquiryCountStockByStockIDAsync(InquiryCountStockByIDQuery inquiryObj)
    {
        return await _httpClientRequest.HttpRequestToObject<InquiryCountStockByIDResponseDTO, InquiryCountStockByIDQuery>(HttpMethod.Post,
           new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/inquiry-countstock-byid"), inquiryObj);
    }
}
