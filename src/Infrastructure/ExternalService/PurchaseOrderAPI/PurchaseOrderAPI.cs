using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.PurchaseOrderAPI;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.ApprovePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.DeletePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.UpdatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderByID.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderByPONumber.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.PurchaseOrderAPI;
public class PurchaseOrderAPI : HttpClientService, IPurchaseOrderAPI
{
    public PurchaseOrderAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreatePurchaseOrderAsync(CreatePurchaseOrderCommand createPurchaseOrderCommand)
    {
		return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreatePurchaseOrderCommand>(HttpMethod.Post,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchase/v1/create"), createPurchaseOrderCommand);
	}

	public async Task<BaseResponse<CommandResponse>> UpdatePurchaseOrderAsync(UpdatePurchaseOrderCommand updatePurchaseOrderCommand)
	{
		return await _httpClientRequest.HttpRequestToObject<CommandResponse, UpdatePurchaseOrderCommand>(HttpMethod.Post,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchase/v1/update"), updatePurchaseOrderCommand);
	}

	public async Task<BaseResponse<CommandResponse>> DeletePurchaseOrderAsync(DeletePurchaseOrderCommand deletePurchaseOrderCommand)
    {
		return await _httpClientRequest.HttpRequestToObject<CommandResponse, DeletePurchaseOrderCommand>(HttpMethod.Post,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchase/v1/delete"), deletePurchaseOrderCommand);
    }

	public async Task<BaseResponse<CommandResponse>> ApprovePurchaseOrderAsync(ApprovePurchaseOrderCommand approvePurchaseOrderCommand)
	{
		return await _httpClientRequest.HttpRequestToObject<CommandResponse, ApprovePurchaseOrderCommand>(HttpMethod.Post,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchase/v1/approve"), approvePurchaseOrderCommand);
	}

	public async Task<BaseResponse<GetPurchaseOrderResposeDTO>> GetPurchaseOrderByIDAsync(int purchaseorderID)
    {
		return await _httpClientRequest.HttpRequestToObject<GetPurchaseOrderResposeDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchase/v1/purchase/{purchaseorderID}"), null);
	}

    public async Task<BaseResponse<GetPurchaseOrderResposeDTO>> GetPurchaseOrderByPONumberAsync(string purchaseOrderNo)
    {
		return await _httpClientRequest.HttpRequestToObject<GetPurchaseOrderResposeDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchase/v1/purchase/{purchaseOrderNo}"), null);
	}

    public async Task<BaseResponse<List<GetPurchaseOrderResposeDTO>>> GetPurchaseOrderListAsync()
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetPurchaseOrderResposeDTO>, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/purchase/v1/purchaselist"), null);
	}

    
}
