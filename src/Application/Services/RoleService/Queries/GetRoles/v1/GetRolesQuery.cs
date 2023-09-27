using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
public record GetRolesQuery : IRequest<BaseResponse<List<GetRolesResponseDTO>>>
{
}
