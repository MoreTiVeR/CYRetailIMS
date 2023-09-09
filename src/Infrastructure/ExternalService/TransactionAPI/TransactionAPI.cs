using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;

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

    public async Task<BaseResponse<List<GetTransactionByBranchIDResponseDTO>>> GetTransactionByBranchIDAsync(int branchid)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetTransactionByBranchIDResponseDTO>,
			CreateTransactionCommand>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/transaction/v1/transactionbybranchid/{branchid}"), null);
	}
}
