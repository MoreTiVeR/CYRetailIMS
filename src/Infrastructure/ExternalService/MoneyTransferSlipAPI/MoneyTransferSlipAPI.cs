using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.MoneyTransferSlipAPI;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByID.v1;
using CYRetailIMS.Application.Services.MoneyTransferSlipService.Quiries.GetSlipByMoneyTransferID.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.MoneyTransferSlipAPI;
public class MoneyTransferSlipAPI : HttpClientService, IMoneyTransferSlipAPI
{
    public MoneyTransferSlipAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetSlipByMoneyTransferIDResponseDTO>> GetMoneyTransferSlipByMoneyTransferIDAsync(GetSlipByMoneyTransferIDQuery moneyTransferIDQuery)
    {
        return await _httpClientRequest.HttpRequestToObject<GetSlipByMoneyTransferIDResponseDTO,
                       GetSlipByMoneyTransferIDQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/slipmoneytransfer/v1/inquirybymoneytransferid"), moneyTransferIDQuery);
    }
}
