using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;

[Serializable]
public class GetUserInBranchByUserIDResponseDTO
{
    public int userid { get; set; }
    public List<GetUserInBranchByUserIDBrancResponseDTO> branchs { get; set; }

}
