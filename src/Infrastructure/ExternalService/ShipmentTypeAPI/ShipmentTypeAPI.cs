using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ShipmentTypeAPI;
using CYRetailIMS.Application.Services.PurchaseTypeService.Queries.GetPurchaseTypeList.v1;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ShipmentTypeAPI;
public class ShipmentTypeAPI : HttpClientService, IShipmentTypeAPI
{
    public ShipmentTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

	public async Task<BaseResponse<List<GetShipmentTypeResponseDTO>>> GetShipmentTypeListAsync()
	{
		return await _httpClientRequest.HttpRequestToObject<List<GetShipmentTypeResponseDTO>, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/shipmenttype/v1/shipmenttypelist"), null);
	}

	public async Task<BaseResponse<GetShipmentTypeResponseDTO>> GetShipmentTypeByIDAsync(int shipmentTypeID)
    {
		return await _httpClientRequest.HttpRequestToObject<GetShipmentTypeResponseDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/shipmenttype/v1/shipmenttype/{shipmentTypeID}"), null);
	}

    
}
