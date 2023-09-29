using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.DeleteEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.EmployeeAPI;
public class EmployeeAPI : HttpClientService, IEmployeeAPI
{
    public EmployeeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateEmployeeAsync(CreateEmployeeCommand createEmployeeCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            CreateEmployeeCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/employee/v1/create"), createEmployeeCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteEmployee(DeleteEmployeeCommand deleteEmployeeCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            DeleteEmployeeCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/employee/v1/delete"), deleteEmployeeCommand);
    }

    public async Task<BaseResponse<GetEmployeeResponseDTO>> GetEmployeeByIDAsync(int empid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetEmployeeResponseDTO,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/employee/v1/getemployeebyid/{empid}"), null);
    }

    public async Task<BaseResponse<List<GetEmployeeResponseDTO>>> GetEmployeesAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetEmployeeResponseDTO>,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/employee/v1/getemployees"), null);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateEmployee(UpdateEmployeeCommand updateEmployeeCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
                    UpdateEmployeeCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/employee/v1/update"), updateEmployeeCommand);
    }
}
