using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.AdjustItemAPI;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.AdjustItemAPI;
public class AdjustItemAPI : HttpClientService, IAdjustItemAPI
{
    public AdjustItemAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateAdjustItemAsync(CreateAdjustItemCommand createAdjustItemCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateAdjustItemCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/adjustitem/v1/create"), createAdjustItemCommand);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateAdjustItemAsync(UpdateAdjustItemCommand updateAdjustItemCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, UpdateAdjustItemCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/adjustitem/v1/update"), updateAdjustItemCommand);
    }
}
