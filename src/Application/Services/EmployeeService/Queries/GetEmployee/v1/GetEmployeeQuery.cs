using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;

[Serializable]
public record GetEmployeeQuery : IRequest<BaseResponse<List<GetEmployeeResponseDTO>>>
{
}
