using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ItemTypeAPI;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ItemTypeAPI;
public class ItemTypeAPI : HttpClientService, IItemTypeAPI
{
    public ItemTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetItemTypeListResponseDTO>> GetItemTypeByIDAsync(int itemtypeid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetItemTypeListResponseDTO, GetItemTypeListQuery>(HttpMethod.Get, 
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtype/v1/getitemtypebyid/{itemtypeid}"), null);
    }

    public async Task<BaseResponse<List<GetItemTypeListResponseDTO>>> GetItemTypeListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetItemTypeListResponseDTO>, GetItemTypeListQuery>(HttpMethod.Get, 
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/itemtype/v1/getitemtypelist"), null);
    }
}
