using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemUnitOfMeasureAPI;
using CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ItemUnitOfMeasureAPI;
public class ItemUnitOfMeasureAPI : HttpClientService, IItemUnitOfMeasureAPI
{
    public ItemUnitOfMeasureAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetUnitOfMeasureListResponseDTO>> GetUnitOfMeasureByIDAsync(int uomid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetUnitOfMeasureListResponseDTO, GetUnitOfMeasureListQuery>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/unitofmeasure/v1/getunitofmeasurebyid/{uomid}"), null);
    }

    public async Task<BaseResponse<List<GetUnitOfMeasureListResponseDTO>>> GetUnitOfMeasureListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetUnitOfMeasureListResponseDTO>, GetUnitOfMeasureListQuery>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/unitofmeasure/v1/getunitofmeasure"), null);
    }
}
