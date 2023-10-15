using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.CurrencyAPI;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByCode.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByID.v1;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.CurrencyAPI;
public class CurrencyAPI : HttpClientService, ICurrencyAPI
{
    public CurrencyAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

	public async Task<BaseResponse<List<GetCurrencyListResponseDTO>>> GetCurrencyListAsync()
	{
		return await _httpClientRequest.HttpRequestToObject<List<GetCurrencyListResponseDTO>, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/currency/v1/currencylist"), null);
	}

	public async Task<BaseResponse<GetCurrencyByIDResponseDTO>> GetCurrencyByIDAsync(int currencyID)
    {
		return await _httpClientRequest.HttpRequestToObject<GetCurrencyByIDResponseDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/currency/v1/currency/{currencyID}"), null);
	}

    public async Task<BaseResponse<GetCurrencyByCodeResponseDTO>> GetCurrencyByCodeAsync(string currencyCode)
    {
		return await _httpClientRequest.HttpRequestToObject<GetCurrencyByCodeResponseDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/currency/v1/currency/{currencyCode}"), null);
	}
}
