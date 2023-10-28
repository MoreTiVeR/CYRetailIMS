using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;

[Serializable]
public record SaleSummaryReportByBranchQuery : IRequest<BaseResponse<SaleSummaryReportResponseDTO>>
{
    public int branchid { get; init; }
    public DateTime transactiondate { get; init; }

}
