using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.SupplierAPI;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.SupplierAPI;
public class SupplierAPI : HttpClientService, ISupplierAPI
{
    public SupplierAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetSupplierResponseDTO>> GetSupplierByIDAsync(int supplierID)
    {
		return await _httpClientRequest.HttpRequestToObject<GetSupplierResponseDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/supplier/v1/supplier/{supplierID}"), null);
		
	}

    public async Task<BaseResponse<List<GetSupplierResponseDTO>>> GetSupplierListAsync()
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetSupplierResponseDTO>, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/supplier/v1/supplierlist"), null);
	}
}
