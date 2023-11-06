using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;

[Serializable]
public class GetRolesResponseDTO
{
    public int roleid { get; set; }

    public string name { get; set; }

    public string description { get; set; }

    public string createdby { get; set; }

    public DateTime createddate { get; set; }

    public bool isactive { get; set; }
}
