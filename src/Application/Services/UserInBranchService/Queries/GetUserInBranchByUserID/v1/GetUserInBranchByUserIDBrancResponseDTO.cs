using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;

[Serializable]
public class GetUserInBranchByUserIDBrancResponseDTO
{
	public int branchid { get; set; }
	public string branchcode { get; set; }
	public string branchname { get; set; }
}
