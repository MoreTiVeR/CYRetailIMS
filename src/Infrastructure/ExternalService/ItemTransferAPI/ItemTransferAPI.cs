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
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatusByID.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;
using static CYRetailIMS.Application.Common.Models.EnumModel;

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

    public async Task<BaseResponse<CommandResponse>> ReceiveItemTransferAsync(UpdateItemTransferCommand receiveItemTransferCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
                    UpdateItemTransferCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/receive"), receiveItemTransferCommand);
    }

    public async Task<BaseResponse<GetItemTransferResponseDTO>> GetItemTransferByIDAsync(int itemTrasferID)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemTransferResponseDTO,
             object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/itemtransfer/{itemTrasferID}"), null);
    }

    public async Task<BaseResponse<List<GetItemTransferResponseDTO>>> GetItemTransferForAdminAsync(GetItemTransferListQuery getItemTransferListQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemTransferResponseDTO>, 
            GetItemTransferListQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/itemtransferforadmin"), getItemTransferListQuery);
    }


    public async Task<BaseResponse<List<GetItemTransferResponseDTO>>> GetItemTransferByCriteriaAsync(GetItemTransferByCriteriaQuery getItemTransferByCriteriaQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemTransferResponseDTO>,
                      GetItemTransferByCriteriaQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/itemtransferbycriteria"), getItemTransferByCriteriaQuery);
    }

    public async Task<BaseResponse<List<GetItemTransferResponseDTO>>> GetItemTransferByDestinationBranchIDAsync(GetItemTransferByDestinationBranchIDQuery getItemTransferByDestinationBranchIDQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemTransferResponseDTO>,
                     GetItemTransferByDestinationBranchIDQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/itemtransferbybranchid"), getItemTransferByDestinationBranchIDQuery);
    }

    public async Task<BaseResponse<List<GetItemTransferStatusResponseDTO>>> GetItemTransferStatusAsync()
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetItemTransferStatusResponseDTO>, 
            GetItemTransferStatusQuery>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/itemtransferstatus"), null);
	}

    public async Task<BaseResponse<GetItemTransferStatusByIDResponseDTO>> GetItemTransferStatusByIDAsync(int transferStatusID)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemTransferStatusByIDResponseDTO,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/itemtransferstatusbyid/{transferStatusID}"), null);
    }

    public async Task<BaseResponse<List<GetTransferTypeListResponseDTO>>> GetItemTransferTypeAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetTransferTypeListResponseDTO>,
             object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/itemtransfertype"), null);
    }

    public async Task<BaseResponse<GetTransferTypeByIDResponseDTO>> GetItemTransferTypeByIDAsync(int transfertypeID)
    {
        return await _httpClientRequest.HttpRequestToObject<GetTransferTypeByIDResponseDTO,
             object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtransfer/v1/itemtransfertypebyid/{transfertypeID}"), null);
    }

   
}
