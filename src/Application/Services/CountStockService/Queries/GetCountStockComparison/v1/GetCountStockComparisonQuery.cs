using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockComparison.v1;

/// <summary>
/// Query for หน้าเทียบข้อมูล - compares system stock vs counted stock
/// with sales/in/out data in a date range.
/// </summary>
public class GetCountStockComparisonQuery : IRequest<BaseResponse<List<GetCountStockComparisonResponseDTO>>>
{
    public int branchid { get; init; }
    public string? subitemtypename { get; init; }

    /// <summary>
    /// วันที่เริ่มต้นสำหรับยอดขาย/สินค้าเข้า/สินค้าออก
    /// </summary>
    public DateTime? salesstartdate { get; init; }
    public DateTime? salesenddate { get; init; }

    /// <summary>
    /// วันที่สำหรับ Audit นับ
    /// </summary>
    public DateTime? auditstartdate { get; init; }
    public DateTime? auditenddate { get; init; }
}
