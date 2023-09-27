using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.UserAPI;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;
using CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.UserAPI;
public class UserAPI : HttpClientService, IUserAPI
{
    public UserAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateUser(CreateEmployeeCommand createEmployeeCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
                    CreateEmployeeCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/user/v1/create"), createEmployeeCommand);
    }

    public async Task<BaseResponse<GetUserResponseDTO>> GetUserByIDAsync(int userid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetUserResponseDTO,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/user/v1/getuserbyid/{userid}"), null);
    }

    public async Task<BaseResponse<List<GetUserResponseDTO>>> GetUsersAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetUserResponseDTO>,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/user/v1/getusers"), null);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateUser(UpdateUserCommand updateUserCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
                    UpdateUserCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/user/v1/update"), updateUserCommand);
    }
}
