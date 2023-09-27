using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.DepartmentAPI;
using CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.DepartmentAPI;
public class DepartmentAPI : HttpClientService, IDepartmentAPI
{
    public DepartmentAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetDepartmentByIDResponseDTO>> GetDepartmentByIDAsync(int departmentid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetDepartmentByIDResponseDTO,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/department/v1/getdepartmentbyid/{departmentid}"), null);
    }

    public async Task<BaseResponse<List<GetDepartmentsResponseDTO>>> GetDepartmentsAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetDepartmentsResponseDTO>,  
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/department/v1/getdepartments"), null);
    }
}
