using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.UserRoleAPI;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.UserRoleAPI;
public class UserRoleAPI : HttpClientService, IUserRoleAPI
{
    public UserRoleAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetRoleByIDResponseDTO>> GetRoleByIDAsync(int roleid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetRoleByIDResponseDTO, 
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/role/v1/getrolebyid/{roleid}"), null);
    }

    public async Task<BaseResponse<List<GetRolesResponseDTO>>> GetRolesAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetRolesResponseDTO>,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/role/v1/getroles"), null);
    }
}
