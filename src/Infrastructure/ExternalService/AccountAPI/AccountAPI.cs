using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.AccountAPI;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.AccountAPI;
public class AccountAPI : HttpClientService, IAccountAPI
{
    public AccountAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<UserProfileResponseDTO>> LoginAsync(LoginQuery loginQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<UserProfileResponseDTO, 
            LoginQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/account/v1/login"), loginQuery);
    }
}
