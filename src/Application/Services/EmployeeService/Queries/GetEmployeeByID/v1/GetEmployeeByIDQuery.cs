using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployeeByID.v1;

[Serializable]
public record GetEmployeeByIDQuery : IRequest<BaseResponse<GetEmployeeResponseDTO>>
{
    public int empid { get; init; }
}
