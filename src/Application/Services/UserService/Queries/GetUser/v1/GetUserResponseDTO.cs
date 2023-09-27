using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;

[Serializable]
public class GetUserResponseDTO
{
    public int userid { get; set; }

    public string username { get; set; }

    public int roleid { get; set; }
    public string rolename { get; set; }

    public string profilepicture { get; set; }

    public DateTime? lastlogin { get; set; }

    public DateTime? lastlogout { get; set; }

    public string createdby { get; set; }

    public DateTime creadeddate { get; set; }

    public bool isactive { get; set; }

    public int? approvestatus { get; set; }

    public string approvestatusname { get; set; }
}
