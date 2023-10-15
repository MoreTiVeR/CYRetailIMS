using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.PaymentTypeAPI;
using CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyList.v1;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.PaymentTypeByID.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.PaymentTypeAPI;
public class PaymentTypeAPI : HttpClientService, IPaymentTypeAPI
{
    public PaymentTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<List<GetPaymentTypeListResponseDTO>>> GetPaymentTypeListAsync()
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetPaymentTypeListResponseDTO>, object>(HttpMethod.Get, 
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/paymenttype/v1/paymenttypelist"), null);
	}

    public async Task<BaseResponse<PaymentTypeByIDResponseDTO>> PaymentTypeByIDAsync(int paymentTypeID)
    {
		return await _httpClientRequest.HttpRequestToObject<PaymentTypeByIDResponseDTO, object>(HttpMethod.Get,
					new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/paymenttype/v1/paymenttype/{paymentTypeID}"), null);
	}
}
