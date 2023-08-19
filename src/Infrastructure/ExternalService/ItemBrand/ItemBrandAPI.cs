using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemBrandAPI;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Infrastructure.ExternalService.ItemBrand;
public class ItemBrandAPI : HttpClientService, IItemBrandAPI
{
    public ItemBrandAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
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
