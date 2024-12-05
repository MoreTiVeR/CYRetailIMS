using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.MoneyTransferAPI;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransferList.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.DeleteMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.UpdateMoneyTransfer.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByCriteria.v1;
using CYRetailIMS.Application.Services.MoneyTransferService.Quiries.GetMoneyTransferByID.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.MoneyTransferAPI;
public class MoneyTransferAPI : HttpClientService, IMoneyTransferAPI
{
    public MoneyTransferAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateAsync(CreateMoneyTransferCommand moneyTransferCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, 
            CreateMoneyTransferCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/moneytransfer/v1/create"), moneyTransferCommand);
    }

    public async Task<BaseResponse<CommandResponse>> BulkCreateAsync(CreateMoneyTransferListCommand moneyTransferCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            CreateMoneyTransferListCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/moneytransfer/v1/bulk-create"), moneyTransferCommand);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateAsync(UpdateMoneyTransferCommand moneyTransferCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, 
            UpdateMoneyTransferCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/moneytransfer/v1/update"), moneyTransferCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteAsync(DeleteMoneyTransferCommand moneyTransferCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            DeleteMoneyTransferCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/moneytransfer/v1/delete"), moneyTransferCommand);
    }

    public async Task<BaseResponse<List<GetMoneyTransferByCriteriaResponseDTO>>> GetMoeytransferByCriteriaAsync(GetMoneyTransferByCriteriaQuery reqData)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetMoneyTransferByCriteriaResponseDTO>,
                    GetMoneyTransferByCriteriaQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/moneytransfer/v1/inquiry"), reqData);
    }

    public async Task<BaseResponse<GetMoneyTransferByCriteriaResponseDTO>> GetMoeytransferByIDAsync(GetMoneyTransferByIDQuery reqData)
    {
        return await _httpClientRequest.HttpRequestToObject<GetMoneyTransferByCriteriaResponseDTO,
                    GetMoneyTransferByIDQuery>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/moneytransfer/v1/inquirybyid"), reqData);
    }


}
