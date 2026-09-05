using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetPendingApprovals.v1;

/// <summary>
/// Query to get list of count stock records pending approval.
/// Admin sees all submitted (status=1) HeadPC records.
/// Filters by counter role and status.
/// </summary>
public class GetPendingApprovalsQuery : IRequest<BaseResponse<List<GetPendingApprovalsResponseDTO>>>
{
    /// <summary>
    /// Filter by counter role: "PC", "HeadPC", or null for all
    /// </summary>
    public string? counterrole { get; init; }

    /// <summary>
    /// Filter by status: 0=Draft,1=Submitted,2=Approved, null=all submitted+approved
    /// </summary>
    public int? statuscid { get; init; }

    /// <summary>
    /// true=เฉพาะรายการจากหน้าบันทึกแบบใหม่ (มี ItemID ใน detail), false=เฉพาะรายการเก่า, null=ทั้งหมด
    /// </summary>
    public bool? isnewentryonly { get; init; }
}
