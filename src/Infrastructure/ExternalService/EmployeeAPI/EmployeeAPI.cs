using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.EmployeeAPI;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;

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
}
