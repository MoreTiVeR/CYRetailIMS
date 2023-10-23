using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.SupplierTypeAPI;
using CYRetailIMS.Application.Services.SupplierTypeService.Queries.GetSupplierTypeList.v1;
using CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.SupplierTypeAPI;
public class SupplierTypeAPI : HttpClientService, ISupplierTypeAPI
{
    public SupplierTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetSupplierTypeResponseDTO>> GetSupplierTypeByIDAsync(int supplierTypeID)
    {
        return await _httpClientRequest.HttpRequestToObject<GetSupplierTypeResponseDTO, object>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/suppliertype/v1/suppliertype/{supplierTypeID}"), null);
    }

    public async Task<BaseResponse<List<GetSupplierTypeResponseDTO>>> GetSupplierTypeListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetSupplierTypeResponseDTO>, object>(HttpMethod.Get,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/suppliertype/v1/suppliertypes"), null);
    }
}
