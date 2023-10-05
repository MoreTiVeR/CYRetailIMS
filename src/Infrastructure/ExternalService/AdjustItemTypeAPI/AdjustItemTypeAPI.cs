using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.AdjustItemTypeAPI;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
using CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemTypeByID.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.AdjustItemTypeAPI;
public class AdjustItemTypeAPI : HttpClientService, IAdjustItemTypeAPI
{
    public AdjustItemTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<List<GetAdjustItemTypeResposeDTO>>> GetAdjustTypesAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetAdjustItemTypeResposeDTO>,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/adjustitemtype/v1/getadjusttypes"), null);
    }

    public async Task<BaseResponse<List<GetAdjustItemTypeByIDResponseDTO>>> GetAdjustTypesAsync(int adjusttypeid)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetAdjustItemTypeByIDResponseDTO>, 
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/adjustitemtype/v1/getadjusttype/{adjusttypeid}"), null);
    }
}
