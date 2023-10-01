using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByTransID.v1;
public record SaleSummaryReportByTransIDQuery : IRequest<BaseResponse<SaleSummaryReportResponseDTO>>
{
    public int transactionid { get; init; }
}
