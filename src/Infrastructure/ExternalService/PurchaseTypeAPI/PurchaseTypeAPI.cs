using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.PurchaseTypeAPI;
using CYRetailIMS.Application.Services.PaymentTypeService.Queries.GetPaymentTypeList.v1;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.PurchaseTypeAPI;
public class PurchaseTypeAPI : HttpClientService, IPurchaseTypeAPI
{
    public PurchaseTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<List<GetPurchaseTypeResponseDTO>>> GetPurchaseTypeListAsync()
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetPurchaseTypeResponseDTO>, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchasetype/v1/purchasetypelist"), null);
	}

    public async Task<BaseResponse<GetPurchaseTypeResponseDTO>> PurchaseTypeByIDAsync(int purchaseTypeID)
    {
		return await _httpClientRequest.HttpRequestToObject<GetPurchaseTypeResponseDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchasetype/v1/purchasetype/{purchaseTypeID}"), null);
	}
}
