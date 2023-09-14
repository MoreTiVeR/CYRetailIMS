using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;

namespace CYRetailIMS.Application.ExternalService.EmployeeAPI;
public interface IEmployeeAPI
{
    Task<BaseResponse<CommandResponse>> CreateEmployeeAsync(CreateEmployeeCommand createEmployeeCommand);
}
