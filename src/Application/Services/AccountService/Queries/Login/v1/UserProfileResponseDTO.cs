using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;

namespace CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;

[Serializable]
public class UserProfileResponseDTO
{
	public int userid { get; set; }
	public int roleid { get; set; }
	public string rolename { get; set; }
	public string username { get; set; }
	public object profilepicture { get; set; }
	public string firstname { get; set; }
	public string lastname { get; set; }
	public string email { get; set; }
	public object lastlogout { get; set; }
	public bool isactive { get; set; }
	public int approvestatus { get; set; }

	public List<GetMenuByRoleIDResponseDTO> access_menu { get; set; }
	//public List<GetMenuByRoleIDResponseDTO> access_menu { get => _access_menu is null ? new List<GetMenuByRoleIDResponseDTO>() : _access_menu; set => _access_menu = value; }

	public List<GetUserInBranchByUserIDBrancResponseDTO> access_branch { get; set; }
	//public List<GetUserInBranchByUserIDBrancResponseDTO> access_branch { get => _access_branch is null ? new List<GetUserInBranchByUserIDBrancResponseDTO>() : _access_branch; set => _access_branch = value; }
}
