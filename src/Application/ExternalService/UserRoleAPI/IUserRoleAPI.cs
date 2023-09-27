using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;

namespace CYRetailIMS.Application.ExternalService.UserRoleAPI;

public interface IUserRoleAPI
{
    Task<BaseResponse<List<GetRolesResponseDTO>>> GetRolesAsync();
    Task<BaseResponse<GetRoleByIDResponseDTO>> GetRoleByIDAsync(int roleid);
}
