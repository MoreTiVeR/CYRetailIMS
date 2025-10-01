using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.ReceiveTempAPI;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
using CYRetailIMS.Application.Services.PurchaseOrderService.Queries.GetPurchaseOrderList.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByBranchID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempByID.v1;
using CYRetailIMS.Application.Services.ReceiveTempService.Queries.GetReceiveTempList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.ReceiveTempAPI;
public class ReceiveTempAPI : HttpClientService, IReceiveTempAPI
{
    public ReceiveTempAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateBranchAsync(CreateReceiveTemplateCommand createReceiveTemplateCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateReceiveTemplateCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/receipttemplate/v1/create"), createReceiveTemplateCommand);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateBranchAsync(UpdateReceiveTemplateCommand updateReceiveTemplateCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, UpdateReceiveTemplateCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/receipttemplate/v1/update"), updateReceiveTemplateCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteBranchAsync(DeleteReceiveTemplateCommand deleteReceiveTemplateCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, DeleteReceiveTemplateCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/receipttemplate/v1/delete"), deleteReceiveTemplateCommand);
    }

    public async Task<BaseResponse<List<GetReceiveTempResponseDTO>>> GetReceiveTemplatehListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetReceiveTempResponseDTO>, object>(HttpMethod.Get,
                new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/receipttemplate/v1/templates"), null);
    }

    public async Task<BaseResponse<GetReceiveTempResponseDTO>> GetReceiveTemplatehByIDAsync(GetReceiveTempByIDQuery objReq)
    {
        return await _httpClientRequest.HttpRequestToObject<GetReceiveTempResponseDTO, object>(HttpMethod.Post,
                        new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/receipttemplate/v1/receivetemplatebyid"), objReq);
    }

    public async Task<BaseResponse<GetReceiveTempResponseDTO>> GetReceiveTemplatehByBranchIDAsync(GetReceiveTempByBranchIDQuery objReq)
    {
        return await _httpClientRequest.HttpRequestToObject<GetReceiveTempResponseDTO, object>(HttpMethod.Post,
                new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/receipttemplate/v1/receivetemplatebybranch"), objReq);
    }

}
