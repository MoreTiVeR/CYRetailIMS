using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ItemInBranchAPI;
public class ItemInBranchAPI : HttpClientService, IItemInBranchAPI
{
    public ItemInBranchAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> UpdateItemInBranchAsync(UpdateItemInBranchCommand updateCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, UpdateItemInBranchCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembranch/v1/update"), updateCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteItemInBranchAsync(DeleteItemInBranchCommand deleteCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, DeleteItemInBranchCommand>(HttpMethod.Post,
     new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembranch/v1/delete"), deleteCommand);
    }

    public async Task<BaseResponse<List<GetItemInBranchListResponseDTO>>> GetItemInBranchAsync()
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetItemInBranchListResponseDTO>, object>(HttpMethod.Get,
			  new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembranch/v1/getiteminbranchlist"), null);
	}

    public async Task<BaseResponse<GetItemInBranchByBranchIDResponseDTO>> GetItemInBranchByBranchIDAsync(int branchID)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemInBranchByBranchIDResponseDTO, object>(HttpMethod.Get,
             new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembranch/v1/getiteminbranchbybranchid/{branchID}"), null);
    }

    public async Task<BaseResponse<List<GetItemInBranchByBranchListResponseDTO>>> GetItemInBranchByBranchListAsync(GetItemInBranchByBranchListQuery queryCommand)
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetItemInBranchByBranchListResponseDTO>, object>(HttpMethod.Post,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembranch/v1/getiteminbranchbybranchidlist"), queryCommand);
	}

    public async Task<BaseResponse<GetItemInBranchByCriteriaResponseDTO>> GetItemInBranchByCriteriaAsync(GetItemInBranchByCriteriaQuery criteriaQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemInBranchByCriteriaResponseDTO, GetItemInBranchByCriteriaQuery>(HttpMethod.Post,
              new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembranch/v1/getiteminbranchbycriteria"), criteriaQuery);
    }

    public async Task<BaseResponse<List<GetItemInventoryTransferResposeDTO>>> GetItemInventoryForTransferAsync(GetItemInventoryTransferQuery inventoryTransferQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemInventoryTransferResposeDTO>, GetItemInventoryTransferQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembranch/v1/getiteminventorytransfer"), inventoryTransferQuery);
    }

    public async Task<BaseResponse<CommandResponse>> CreateItemInBranchListAsync(CreateItemInBranchListCommand createCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateItemInBranchListCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembranch/v1/bulkcreate"), createCommand);
    }
}
