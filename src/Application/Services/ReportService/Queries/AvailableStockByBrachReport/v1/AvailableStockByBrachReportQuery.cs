using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockByBrachReport.v1;

[Serializable]
public record AvailableStockByBrachReportQuery : IRequest<BaseResponse<List<AvailableStockReportResponseDTO>>>
{
    public int branchid { get; init; }
}
