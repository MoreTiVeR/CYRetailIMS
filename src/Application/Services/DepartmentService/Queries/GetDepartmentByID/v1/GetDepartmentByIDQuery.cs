using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;

[Serializable]
public record GetDepartmentByIDQuery : IRequest<BaseResponse<GetDepartmentByIDResponseDTO>>
{
    public int departmentid { get; init; }
}
