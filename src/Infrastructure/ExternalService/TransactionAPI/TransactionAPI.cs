using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.TransactionAPI;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;

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
}
