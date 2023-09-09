using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;

[Serializable]
public class GetItemInBranchByBranchIDResponseDTO
{
	public int branchid { get; set; }

	public string branchname { get; set; }

	public List<GetItemInBranchByBranchIDItemResponseDTO> itemlist { get; set; }
}
