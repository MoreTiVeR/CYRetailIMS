using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;

[Serializable]
public record AvailableStockReportQuery : IRequest<BaseResponse<List<AvailableStockReportResponseDTO>>>
{
    public int? branchid { get; init; }
}
