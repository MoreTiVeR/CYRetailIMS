using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryReport.v1;
public record InventoryReportQuery : IRequest<BaseResponse<List<InventoryReportResponseDTO>>>
{
    public DateTime reportdate { get; init; }
    public int? branchid { get; init; }
}
