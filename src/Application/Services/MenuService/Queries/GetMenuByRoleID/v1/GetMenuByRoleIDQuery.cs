using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
public record GetMenuByRoleIDQuery : IRequest<BaseResponse<List<GetMenuByRoleIDResponseDTO>>>
{
    [Required]
    public int RoleID { get; init; }
}
