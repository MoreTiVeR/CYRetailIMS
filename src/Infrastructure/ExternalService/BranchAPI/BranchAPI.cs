using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Commands.DeleteBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Commands.UpdateBranch.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;
using CYRetailIMS.Application.Services.ItemBrandService.Commands.CreateBrand.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.BranchAPI;
public class BranchAPI : HttpClientService, IBranchAPI
{
    public BranchAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
    }

    public async Task<BaseResponse<CommandResponse>> CreateBranchAsync(CreateBranchCommand createBranchCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
            CreateBranchCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/branch/v1/create"), createBranchCommand);
    }

    public async Task<BaseResponse<CommandResponse>> UpdateBranchAsync(UpdateBranchCommand updateBranchCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
                    UpdateBranchCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/branch/v1/update"), updateBranchCommand);
    }

    public async Task<BaseResponse<CommandResponse>> DeleteBranchAsync(DeleteBranchCommand deleteBranchCommand)
    {
        return await _httpClientRequest.HttpRequestToObject<CommandResponse,
                    DeleteBranchCommand>(HttpMethod.Post, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/branch/v1/delete"), deleteBranchCommand);
    }

    public async Task<BaseResponse<GetBranchByIDResponseDTO>> GetBranchByIDAsync(int branchID)
    {
        return await _httpClientRequest.HttpRequestToObject<GetBranchByIDResponseDTO,
            GetBranchByIDQuery>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/branch/v1/getbranchbyid/{branchID}"), null);
    }

    public async Task<BaseResponse<List<GetBranchListResponseDTO>>> GetBranchListAsync()
    {
        return await _httpClientRequest.HttpRequestToObject<List<GetBranchListResponseDTO>, 
            GetBranchListQuery>(HttpMethod.Get, new Uri($"{_httpClientRequest.CYApiUrl}/api/v1/branch/v1/getbranchlist"), null);
    }
}
