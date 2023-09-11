using CYRetailIMS.Application.Common.Interfaces;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.ExternalService.BranchAPI;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
using CYRetailIMS.Application.Services.BranchService.Queries.GetBranchList.v1;

namespace CYRetailIMS.Infrastructure.ExternalService.BranchAPI;
public class BranchAPI : HttpClientService, IBranchAPI
{
    public BranchAPI(ILog4NetLogger log, IHttpClientRequest httpClientRequest) : base(log, httpClientRequest)
    {
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
