using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;

[Serializable]
public record SaleReportQuery : IRequest<BaseResponse<List<SaleReportResponseDTO>>>
{
    public int? branchid { get; init; }
    public DateTime transaction_startdate { get; init; }
    public DateTime transaction_enddate { get; init; }
}
