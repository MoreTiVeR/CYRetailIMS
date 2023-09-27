using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;

[Serializable]
public record GetRoleByIDQuery : IRequest<BaseResponse<GetRoleByIDResponseDTO>>
{
    public int roleid { get; init; }
}
