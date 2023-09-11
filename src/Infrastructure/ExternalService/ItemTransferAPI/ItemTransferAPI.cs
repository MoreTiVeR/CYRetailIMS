using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemTransferAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;

namespace CYRetailIMS.Infrastructure.ExternalService.ItemTransferAPI;
public class ItemTransferAPI : HttpClientService, IItemTransferAPI
{
    public ItemTransferAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateItemTransferAsync(CreateItemTransferCommand createItemTransferCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            CreateItemTransferCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/create"), createItemTransferCommand);
    }

    public async Task<BaseResponse<GetItemTransferResponseDTO>> GetItemTransferByIDAsync(int itemTrasferID)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemTransferResponseDTO,
             object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/{itemTrasferID}"), null);
    }

    public async Task<BaseResponse<List<GetItemTransferResponseDTO>>> GetItemTransferListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemTransferResponseDTO>,
              object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransferlist"), null);
    }
}
