using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchList.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;

namespace CYRetailIMS.Application.ExternalService.ItemInBranchAPI;
public interface IItemInBranchAPI
{
	Task<BaseResponse<List<GetItemInBranchListResponseDTO>>> GetItemInBranchAsync();
	Task<BaseResponse<GetItemInBranchByBranchIDResponseDTO>> GetItemInBranchByBranchIDAsync(int branchID);
	Task<BaseResponse<List<GetItemInBranchByBranchListResponseDTO>>> GetItemInBranchByBranchListAsync(GetItemInBranchByBranchListQuery queryCommand);
	Task<BaseResponse<GetItemInBranchByCriteriaResponseDTO>> GetItemInBranchByCriteriaAsync(GetItemInBranchByCriteriaQuery criteriaQuery);
    Task<BaseResponse<CommandResponse>> UpdateItemInBranchAsync(UpdateItemInBranchCommand updateCommand);
    Task<BaseResponse<CommandResponse>> DeleteItemInBranchAsync(DeleteItemInBranchCommand deleteCommand);
    Task<BaseResponse<List<GetItemInventoryTransferResposeDTO>>> GetItemInventoryForTransferAsync(GetItemInventoryTransferQuery inventoryTransferQuery);
}
