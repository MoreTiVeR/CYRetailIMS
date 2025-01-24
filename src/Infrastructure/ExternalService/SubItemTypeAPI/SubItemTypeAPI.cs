using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.SubItemTypeAPI;
using CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByID.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByItemIDList.v1;
using CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.SubItemTypeAPI;
public class SubItemTypeAPI : HttpClientService, ISubItemTypeAPI
{
    public SubItemTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateSubItemTypeAsync(CreateSubItemTypeCommand subItemTypeCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateSubItemTypeCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/subitemtype/v1/create"), subItemTypeCommand);
    }

    public async Task<BaseResponse<GetSubItemTypeResponseDTO>> GetSubItemTypeByIDAsync(GetSubItemTypeByIDQuery subItemTypeByIDQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<GetSubItemTypeResponseDTO, GetSubItemTypeByIDQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/subitemtype/v1/subitemtypebyid"), subItemTypeByIDQuery);
    }

    public async Task<BaseResponse<List<GetSubItemTypeResponseDTO>>> GetSubItemTypeListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetSubItemTypeResponseDTO>, GetSubItemTypeListQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/subitemtype/v1/subitemtypelist"), null);
    }

    public async Task<BaseResponse<List<GetSubItemTypeByItemIDListResponseDTO>>> GetSubItemTypeByItemIDListAsync(GetSubItemTypeByItemIDListQuery listQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetSubItemTypeByItemIDListResponseDTO>, GetSubItemTypeByItemIDListQuery>(HttpMethod.Post,
                   new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/subitemtype/v1/subitemtypebyitemids"), listQuery);
    }
}
