using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ItemAPI;
public class ItemAPI : HttpClientService, IItemAPI
{
    public ItemAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest)
        : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateItemAsync(CreateItemCommand createItemCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            CreateItemCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/create"), createItemCommand);
    }

	public async Task<BaseResponse<CommandResponse>> CreateItemListAsync(CreateItemListCommand createItemListCommand)
	{
		return await _httpClientRequest.HttpRequestToObject<CommandResponse, 
            CreateItemListCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/createlist"), createItemListCommand);
	}

	public async Task<BaseResponse<CommandResponse>> UpdateItemAsync(UpdateItemCommand updateItemCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            UpdateItemCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/update"), updateItemCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteItemAsync(DeleteItemCommand deleteItemCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, 
            DeleteItemCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/delete"), deleteItemCommand);
    }

    public async Task<BaseResponse<GetItemListResponseDTO>> GetItemByIdAsync(int itemID)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemListResponseDTO, object>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/getitembyid/{itemID}"), null);
    }

    public async Task<BaseResponse<List<GetItemListResponseDTO>>> GetItemListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemListResponseDTO>, object>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/getitemlist"), null);
    }

    public async Task<BaseResponse<GetItemByIDResponseDTO>> GetItemByBarCodeAsync(string itemBarcode)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemByIDResponseDTO, object>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/getitembybarcode/{itemBarcode}"), null);
    }
}
