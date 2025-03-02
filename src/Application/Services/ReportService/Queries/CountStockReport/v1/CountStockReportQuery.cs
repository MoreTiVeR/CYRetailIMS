using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.CountStockReport.v1;
public record CountStockReportQuery : IRequest<BaseResponse<List<CountStockReportResponseDTO>>>
{
    public int? branchid { get; init; }
    public DateTime? startdate { get; init; }
    public DateTime? enddate { get; init; }
    public int? subitemtypeid { get; init; }
}
