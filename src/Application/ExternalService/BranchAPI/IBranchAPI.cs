using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;

namespace CYRetailIMS.Application.ExternalService.BranchAPI;
public interface IBranchAPI
{
    Task<BaseResponse<List<GetBranchListResponseDTO>>> GetBranchListAsync();

    Task<BaseResponse<GetBranchByIDResponseDTO>> GetBranchByIDAsync(int branchID);
}
