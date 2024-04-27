using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;
using CYRetailIMS.Application.Services.TransactionService.Commands.UpdateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;

namespace CYRetailIMS.Application.ExternalService.TransactionAPI;
public interface ITransactionAPI
{
    Task<BaseResponse<CommandResponse>> CreateTransactionAsync(CreateTransactionCommand createItemCommand);

    Task<BaseResponse<CommandResponse>> DeleteTransactionByIDAsync(DeleteTransactionCommand deleteTransactionCommand);

    Task<BaseResponse<CommandResponse>> UpdateTransactionAsync(UpdateTransactionCommand updateTransactionCommand);

    Task<BaseResponse<List<GetTransactionByBranchIDResponseDTO>>> GetTransactionByBranchIDAsync(int branchid);

    Task<BaseResponse<GetTransactionByBranchIDResponseDTO>> GetTransactionByIDAsync(int transactionid);
}
