using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ReceiptAPI;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceipt.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.GenerateReceiptNo.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ReceiptAPI;
public class ReceiptAPI : HttpClientService, IReceiptAPI
{
    public ReceiptAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateReceiptAsync(CreateReceiptCommand createReceiptCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateReceiptCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/receipt/v1/create"), createReceiptCommand);
    }

    public async Task<BaseResponse<GenerateReceiptNoResponseDTO>> GenerateReceiptNoByBranchAsync(GenerateReceiptNoCommand generateReceiptNoCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<GenerateReceiptNoResponseDTO, GenerateReceiptNoCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/receipt/v1/generate-receiptno"), generateReceiptNoCommand);
    }
}
