using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.SupplierContactTypeAPI;
using CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeList.v1;
using CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.SupplierContactTypeAPI;
public class SupplierContactTypeAPI : HttpClientService, ISupplierContactTypeAPI
{
    public SupplierContactTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetSupplierContactTypeResposeDTO>> GetSupplierContactTypeByIDAsync(int supplierID)
    {
		return await _httpClientRequest.HttpRequestToObject<GetSupplierContactTypeResposeDTO, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/suppliercontacttype/v1/suppliercontacttype/{supplierID}"), null);
	}

    public async Task<BaseResponse<List<GetSupplierContactTypeResposeDTO>>> GetSupplierContactTypeListAsync()
    {
		return await _httpClientRequest.HttpRequestToObject<List<GetSupplierContactTypeResposeDTO>, object>(HttpMethod.Get,
			new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/suppliercontacttype/v1/suppliercontacttypelist"), null);
	}
}
