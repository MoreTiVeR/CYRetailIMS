using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
using CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByTransferID.v1;

namespace CYRetailIMS.Application.ExternalService.ItemTransferAPI;
public interface IItemTransferAPI
{
    Task<BaseResponse<CommandResponse>> CreateItemTransferAsync(CreateItemTransferCommand createItemTransferCommand);
    Task<BaseResponse<GetItemTransferResponseDTO>> GetItemTransferByIDAsync(int itemTrasferID);
    Task<BaseResponse<List<GetItemTransferResponseDTO>>> GetItemTransferListAsync();
}
