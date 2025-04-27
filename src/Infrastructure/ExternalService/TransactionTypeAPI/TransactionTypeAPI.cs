
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.TransactionTypeAPI;
using CYRetailIMS.Application.Services.TransactionTypeService.Queries.GetTrasnactionList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.TransactionTypeAPI;
public class TransactionTypeAPI : HttpClientService, ITransactionTypeAPI
{
    public TransactionTypeAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<List<GetTrasnactionByCriteriaResponseDTO>>> GetTransactionTypeByCriteriaAsync(GetTrasnactionByCriteriaQuery reqObj)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetTrasnactionByCriteriaResponseDTO>, object>(HttpMethod.Post, 
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/transactiontype/v1/inquiry"), reqObj);
    }

}
