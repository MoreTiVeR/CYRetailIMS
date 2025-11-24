using System;
using System.Net.Http;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.EndOfDaySummaryAPI;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.CreateEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.DeleteEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.UpdateEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByID.v1;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.EndOfDaySummaryAPI;
public class EndOfDaySummaryAPI : HttpClientService, IEndOfDaySummaryAPI
{
    public EndOfDaySummaryAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<GetEndOfDaySummaryByCriteriaResponseDTO>> GetEndOfDaySummaryByCriteriaAsync(GetEndOfDaySummaryByCriteriaQuery request)
    {
        return await _httpClientRequest.HttpRequestToObject<GetEndOfDaySummaryByCriteriaResponseDTO, GetEndOfDaySummaryByCriteriaQuery>(
            HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/eodsummary/v1/inquiry"),
            request);
    }

    public async Task<BaseResponse<GetEndOfDaySummaryByCriteriaDetail>> SearchEndOfDaySummaryByIDAsync(GetEndOfDaySummaryByIDQuery request)
    {
        return await _httpClientRequest.HttpRequestToObject<GetEndOfDaySummaryByCriteriaDetail, GetEndOfDaySummaryByIDQuery>(HttpMethod.Post, 
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/eodsummary/v1/search"), request);
    }

    public async Task<BaseResponse<CommandResponse>> CreateEndOfDaySummaryAsync(CreateEndOfDaySummaryCommand request)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, CreateEndOfDaySummaryCommand>(
            HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/eodsummary/v1/create"),
            request);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateEndOfDaySummaryAsync(UpdateEndOfDaySummaryCommand request)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, UpdateEndOfDaySummaryCommand>(
            HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/eodsummary/v1/update"),
            request);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteEndOfDaySummaryAsync(DeleteEndOfDaySummaryCommand request)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse, DeleteEndOfDaySummaryCommand>(
            HttpMethod.Post,
            new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/eodsummary/v1/delete"),
            request);
    }
}