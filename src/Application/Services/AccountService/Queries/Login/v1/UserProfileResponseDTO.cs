using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;

namespace CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
public class UserProfileResponseDTO
{
    [JsonPropertyName("userid")]
    public int UserID { get; set; }

    [JsonPropertyName("roleid")]
    public int RoleID { get; set; }

    [JsonPropertyName("username")]
    public string UserName { get; set; }

    [JsonPropertyName("profilepicture")]
    public string ProfilePicture { get; set; }

    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastname")]
    public string LastName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("lastlogout")]
    public DateTime? LastLogout { get; set; }

    [JsonPropertyName("isactive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("approvestatus")]
    public int? ApproveStatus { get; set; }

    [JsonPropertyName("access_menu")]
    public List<GetMenuByRoleIDResponseDTO> access_menu { get; set; }
}
