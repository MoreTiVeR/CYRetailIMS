using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReport.v1;

[Serializable]
public record SaleSummaryReportQuery : IRequest<BaseResponse<List<SaleSummaryReportResponseDTO>>>
{
	public DateTime transactiondate { get; init; }
	public int? branchid { get; init; }
	
}
