using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.DeleteEmployee.v1;
public record DeleteEmployeeCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int empid { get; init; }
    public string updatedby { get; init; }
}
