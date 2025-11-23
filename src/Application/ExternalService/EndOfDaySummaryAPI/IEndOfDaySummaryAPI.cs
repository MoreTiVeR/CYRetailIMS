using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.CreateEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Commands.UpdateEndOfDaySummary;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryByCriteria.v1;
using CYRetailIMS.Application.Services.EODSummaryService.Queries.GetEndOfDaySummaryList.v1;

namespace CYRetailIMS.Application.ExternalService.EndOfDaySummaryAPI;
public interface IEndOfDaySummaryAPI
{
    Task<BaseResponse<GetEndOfDaySummaryByCriteriaResponseDTO>> GetEndOfDaySummaryByCriteriaAsync(GetEndOfDaySummaryByCriteriaQuery request);
    Task<BaseResponse<CommandResponse>> CreateEndOfDaySummaryAsync(CreateEndOfDaySummaryCommand request);
    Task<BaseResponse<CommandResponse>> UpdateEndOfDaySummaryAsync(UpdateEndOfDaySummaryCommand request);
}