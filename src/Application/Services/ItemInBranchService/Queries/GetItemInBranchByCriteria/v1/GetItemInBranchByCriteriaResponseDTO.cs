using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;

[Serializable]
public class GetItemInBranchByCriteriaResponseDTO
{
    public int branchid { get; set; }

    public string branchname { get; set; }

    public GetItemInBranchByBranchIDItemResponseDTO item { get; set; }
}
