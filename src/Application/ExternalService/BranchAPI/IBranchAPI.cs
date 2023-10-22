using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Commands.DeleteBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Commands.UpdateBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;

namespace CYRetailIMS.Application.ExternalService.BranchAPI;
public interface IBranchAPI
{
    Task<BaseResponse<CommandResponse>> CreateBranchAsync(CreateBranchCommand createBranchCommand);
    Task<BaseResponse<CommandResponse>> UpdateBranchAsync(UpdateBranchCommand updateBranchCommand);
    Task<BaseResponse<CommandResponse>> DeleteBranchAsync(DeleteBranchCommand deleteBranchCommand);
    Task<BaseResponse<List<GetBranchResponseDTO>>> GetBranchListAsync();
    Task<BaseResponse<GetBranchResponseDTO>> GetBranchByIDAsync(int branchID);
}
