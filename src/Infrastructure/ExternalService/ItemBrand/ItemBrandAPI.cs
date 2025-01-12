using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.CreateBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.DeleteBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.UpdateBrand.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Infrastructure.ExternalService.ItemBrand;
public class ItemBrandAPI : HttpClientService, IItemBrandAPI
{
    public ItemBrandAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateItemBrandAsync(CreateBrandCommand createBrandCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateBrandCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/create"), createBrandCommand);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateItemBrandAsync(UpdateBrandCommand updateBrandCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, UpdateBrandCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/update"), updateBrandCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteItemBrandAsync(DeleteBrandCommand deleteBrandCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, DeleteBrandCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/delete"), deleteBrandCommand);
    }

    public async Task<BaseResponse<GetItemBrandListResponseDTO>> GetItemBrandByIDAsync(int brandid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemBrandListResponseDTO, GetItemBrandListQuery>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/getitembrandbyid/{brandid}"), null);
    }

    public async Task<BaseResponse<List<GetItemBrandListResponseDTO>>> GetItemBrandListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemBrandListResponseDTO>, GetItemBrandListQuery>(HttpMethod.Get, 
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itembrand/v1/getitembrandlist"), null);
    }

   
}
