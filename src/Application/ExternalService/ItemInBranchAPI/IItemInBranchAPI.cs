using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;

namespace CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
public interface IItemInBranchAPI
{
    Task<BaseResponse<GetItemInBranchByBranchIDResponseDTO>> GetItemInBranchByBranchIDAsync(int branchID);
}
