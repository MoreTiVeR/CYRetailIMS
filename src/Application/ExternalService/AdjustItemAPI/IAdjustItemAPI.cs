using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactionByID.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactions.v1;

namespace CYRetailIMS.Application.ExternalService.AdjustItemAPI;
public interface IAdjustItemAPI
{
    Task<BaseResponse<CommandResponse>> CreateAdjustItemAsync(CreateAdjustItemCommand createAdjustItemCommand);

    Task<BaseResponse<CommandResponse>> UpdateAdjustItemAsync(UpdateAdjustItemCommand updateAdjustItemCommand);

    Task<BaseResponse<List<GetAdjustItemTransactionsResponseDTO>>> GetAdjustItemTransactionAsync();

    Task<BaseResponse<GetAdjustItemTransactionByIDResponseDTO>> GetAdjustItemTransactionByIDAsync(int adjusttransactionID);

    Task<BaseResponse<List<GetAdjustItemTransactionsResponseDTO>>> GetAdjustItemTransactionByBranchIDAsync(int branhID);
}
