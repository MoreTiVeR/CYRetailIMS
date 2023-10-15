using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.WarehouseAPI;
using CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;
using CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.WarehouseAPI;
public class WarehouseAPI : HttpClientService, IWarehouseAPI
{
    public WarehouseAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetWarehouseResponseDTO>> GetWarehouseByIDAsync(int warehouseID)
    {
		return await _httpClientRequest.HttpRequestToObject<GetWarehouseResponseDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/warehouse/v1/warehouse/{warehouseID}"), null);
	}

    public async Task<BaseResponse<List<GetWarehouseResponseDTO>>> GetWarehouseListAsync()
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetWarehouseResponseDTO>, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/warehouse/v1/warehouselist"), null);
	}
}
