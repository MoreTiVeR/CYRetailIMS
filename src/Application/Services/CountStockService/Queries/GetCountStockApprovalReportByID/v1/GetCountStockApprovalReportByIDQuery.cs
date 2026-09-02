using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReportByID.v1;

public class GetCountStockApprovalReportByIDQuery : IRequest<BaseResponse<GetCountStockApprovalReportByIDResponseDTO>>
{
    public int countstockid { get; init; }
}
