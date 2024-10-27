using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByDraftID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransferFromDraft.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateDraftItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetDraftItemTransferByCriteria.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferList.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatus.v1;
using CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatusByID.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
using CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeList.v1;

namespace CYRetailIMS.Application.ExternalService.ItemTransferAPI;
public interface IItemTransferAPI
{
    Task<BaseResponse<CommandResponse>> CreateItemTransferAsync(CreateItemTransferCommand createItemTransferCommand);

    Task<BaseResponse<CommandResponse>> CreateItemTransferFromDrafAsyc(CreateItemTransferFromDraftCommand reqObj);

    Task<BaseResponse<CommandResponse>> CreateDraftItemTransferAsync(CreateDraftItemTransferCommand createDraftItemTransferCommand);

    Task<BaseResponse<CommandResponse>> UpdateDraftItemTransferAsync(UpdateDraftItemTransferCommand updateDraftItemTransferCommand);

    Task<BaseResponse<CommandResponse>> ReceiveItemTransferAsync(UpdateItemTransferCommand receiveItemTransferCommand);

    Task<BaseResponse<GetItemTransferResponseDTO>> GetItemTransferByIDAsync(int itemTrasferID);

    Task<BaseResponse<List<GetItemTransferResponseDTO>>> GetItemTransferForAdminAsync(GetItemTransferListQuery getItemTransferListQuery);

    Task<BaseResponse<List<GetItemTransferResponseDTO>>> GetItemTransferByCriteriaAsync(GetItemTransferByCriteriaQuery getItemTransferByCriteriaQuery);

    Task<BaseResponse<List<GetItemTransferResponseDTO>>> GetItemTransferByDestinationBranchIDAsync(GetItemTransferByDestinationBranchIDQuery getItemTransferByDestinationBranchIDQuery);

    Task<BaseResponse<List<GetItemTransferStatusResponseDTO>>> GetItemTransferStatusAsync();

    Task<BaseResponse<GetItemTransferStatusByIDResponseDTO>> GetItemTransferStatusByIDAsync(int transferStatusID);

    Task<BaseResponse<List<GetTransferTypeListResponseDTO>>> GetItemTransferTypeAsync();

    Task<BaseResponse<GetTransferTypeByIDResponseDTO>> GetItemTransferTypeByIDAsync(int transfertypeID);

    Task<BaseResponse<GetDraftItemTransferByBranchIDResponseDTO>> GetDraftItemTransferByBranchIDAsync(GetDraftItemTransferByBranchIDQuery reqObj);
    Task<BaseResponse<List<GetDraftItemTransferByBranchIDResponseDTO>>> GetDraftItemTransferByCriteriaAsync(GetDraftItemTransferByCriteriaQuery reqObj);
    Task<BaseResponse<List<GetItemInventoryTransferResposeDTO>>> InquiryDraftItemTransferByDraftIDAsync(GetItemInventoryForTransferByDraftIDQuery reqObj);
}
