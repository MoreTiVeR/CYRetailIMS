using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;

namespace CYRetailIMS.Application.ExternalService.EmployeeAPI;
public interface IEmployeeAPI
{
    Task<BaseResponse<CommandResponse>> CreateEmployeeAsync(CreateEmployeeCommand createEmployeeCommand);

    Task<BaseResponse<List<GetEmployeeResponseDTO>>> GetEmployeesAsync();

    Task<BaseResponse<GetEmployeeResponseDTO>> GetEmployeeByIDAsync(int empid);

    Task<BaseResponse<CommandResponse>> UpdateEmployee(UpdateEmployeeCommand updateEmployeeCommand);
}
