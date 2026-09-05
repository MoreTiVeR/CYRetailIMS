using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.CountStockAPI;
using CYRetailIMS.Application.Services.CountStockService.Commands.ApproveCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.CancelCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.DeleteCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.SubmitCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Commands.UpdateCountStock.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReport.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReportByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockComparison.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.GetPendingApprovals.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
using CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
using CreateCountStockCommandV1 = CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1.CreateCountStockCommand;
using CreateCountStockCommandV2 = CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v2.CreateCountStockCommand;

namespace CYRetailIMS.Infrastructure.ExternalService.CountStockAPI;
public class CountStockAPI : HttpClientService, ICountStockAPI
{
    public CountStockAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateCountStockListAsync(CreateCountStockCommandV1 createCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateCountStockCommandV1>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/create"), createCommand);
    }

    public async Task<BaseResponse<CommandResponse>> CreateCountStockListV2Async(CreateCountStockCommandV2 createCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateCountStockCommandV2>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v2/stock/v2/create"), createCommand);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateCountStocAsync(UpdateCountStockCommand updateCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, UpdateCountStockCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/update"), updateCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteCountStockAsync(DeleteCountStockCommand deleteCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, DeleteCountStockCommand>(HttpMethod.Post,
                    new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/delete"), deleteCommand);
    }

    public async Task<BaseResponse<List<InquiryCountStockResponseDTO>>> GetCountStockListAsync(InquiryCountStocksQuery inquiryObj)
    {
        return await _httpClientRequest.HttpRequestToObject<List<InquiryCountStockResponseDTO>, InquiryCountStocksQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/inquiry"), inquiryObj);
    }

    public async Task<BaseResponse<List<InquiryCountStockByBranchIDResponseDTO>>> InquiryCountStockByBranchIDAsync(InquiryCountStockByBranchIDQuery inquiryObj)
    {
        return await _httpClientRequest.HttpRequestToObject<List<InquiryCountStockByBranchIDResponseDTO>, InquiryCountStockByBranchIDQuery>(HttpMethod.Post,
           new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/inquiry-countstock-bybranch"), inquiryObj);
    }

    public async Task<BaseResponse<InquiryCountStockByIDResponseDTO>> InquiryCountStockByStockIDAsync(InquiryCountStockByIDQuery inquiryObj)
    {
        return await _httpClientRequest.HttpRequestToObject<InquiryCountStockByIDResponseDTO, InquiryCountStockByIDQuery>(HttpMethod.Post,
           new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/inquiry-countstock-byid"), inquiryObj);
    }

    public async Task<BaseResponse<CommandResponse>> SubmitCountStockAsync(SubmitCountStockCommand submitCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, SubmitCountStockCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/submit"), submitCommand);
    }

    public async Task<BaseResponse<CommandResponse>> CancelCountStockAsync(CancelCountStockCommand cancelCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CancelCountStockCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/cancel"), cancelCommand);
    }

    public async Task<BaseResponse<CommandResponse>> ApproveCountStockAsync(ApproveCountStockCommand approveCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, ApproveCountStockCommand>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/approve"), approveCommand);
    }

    public async Task<BaseResponse<List<GetPendingApprovalsResponseDTO>>> GetPendingApprovalsAsync(GetPendingApprovalsQuery query)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetPendingApprovalsResponseDTO>, GetPendingApprovalsQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/pending-approvals"), query);
    }

    public async Task<BaseResponse<List<GetCountStockComparisonResponseDTO>>> GetCountStockComparisonAsync(GetCountStockComparisonQuery query)
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetCountStockComparisonResponseDTO>, GetCountStockComparisonQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/comparison"), query);
    }

    public async Task<BaseResponse<GetCountStockApprovalReportResponseDTO>> GetCountStockApprovalReportAsync(GetCountStockApprovalReportQuery query)
    {
        return await _httpClientRequest.HttpRequestToObject<GetCountStockApprovalReportResponseDTO, GetCountStockApprovalReportQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/approval-history"), query);
    }

    public async Task<BaseResponse<GetCountStockApprovalReportByIDResponseDTO>> GetCountStockApprovalReportByIDAsync(GetCountStockApprovalReportByIDQuery query)
    {
        return await _httpClientRequest.HttpRequestToObject<GetCountStockApprovalReportByIDResponseDTO, GetCountStockApprovalReportByIDQuery>(HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/stock/v1/approval-history-detail"), query);
    }
}

