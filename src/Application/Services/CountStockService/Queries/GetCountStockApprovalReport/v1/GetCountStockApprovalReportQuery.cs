using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockApprovalReport.v1;

public class GetCountStockApprovalReportQuery : IRequest<BaseResponse<GetCountStockApprovalReportResponseDTO>>
{
    public int? branchid { get; init; }
    public DateTime? startdate { get; init; }
    public DateTime? enddate { get; init; }
    public int startrow { get; init; }
    public int pagesize { get; init; }
    public string? searchvalue { get; init; }
    public bool isexportalldata { get; init; }
}
