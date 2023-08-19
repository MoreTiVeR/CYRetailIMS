using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.ItemService.Commands.DeleteItem;
using CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
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

    public async Task<BaseResponse<GetItemListResponseDTO>> GetItemByIdAsync(int itemid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemListResponseDTO, GetItemListQuery>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/getitembyid/{itemid}"), null);
    }

    public async Task<BaseResponse<List<GetItemListResponseDTO>>> GetItemListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemListResponseDTO>, GetItemListQuery>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/item/v1/getitemlist"), null);
    }

    
}
