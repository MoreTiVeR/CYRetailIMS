using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;
using CYRetailIMS.Application.Services.TransactionService.Commands.UpdateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByTransactionID.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.TransactionAPI;
public class TransactionAPI : HttpClientService, ITransactionAPI
{
    public TransactionAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateTransactionAsync(CreateTransactionCommand createItemCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, 
            CreateTransactionCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/transaction/v1/create"), createItemCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteTransactionByIDAsync(DeleteTransactionCommand deleteTransactionCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, 
            DeleteTransactionCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/transaction/v1/delete"), deleteTransactionCommand);
    }
    public async Task<BaseResponse<CommandResponse>> UpdateTransactionAsync(UpdateTransactionCommand updateTransactionCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            UpdateTransactionCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/transaction/v1/update"), updateTransactionCommand);
    }

    public async Task<BaseResponse<List<GetTransactionByBranchIDResponseDTO>>> GetTransactionByBranchIDAsync(int branchid)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetTransactionByBranchIDResponseDTO>,
            GetTransactionByBranchIDQuery>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/transaction/v1/transactionbybranchid/{branchid}"), null);
	}

    public async Task<BaseResponse<GetTransactionByBranchIDResponseDTO>> GetTransactionByIDAsync(int transactionid)
    {
        return await _httpClientRequest.HttpRequestToObject<GetTransactionByBranchIDResponseDTO,
            object>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/transaction/v1/transactionbyid/{transactionid}"), null);
    }


}
