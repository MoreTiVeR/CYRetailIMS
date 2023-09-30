using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AuditReport.v1;
public record AuditReportQuery : IRequest<BaseResponse<List<AuditReportResponseDTO>>>
{
	public DateTime transaction_startdate { get; init; }
	public DateTime transaction_enddate { get; init; }
}
