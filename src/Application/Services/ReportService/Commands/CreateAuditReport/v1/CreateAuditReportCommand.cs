using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Commands.CreateAuditReport.v1;

[Serializable]
public record CreateAuditReportCommand : IRequest<BaseResponse<CommandResponse>>
{
    public int branchid { get; init; }
    public decimal totalamountaudit { get; init; }
    public string description { get; init; }
    public DateTime transactiondatetime { get; init; }
    public string createdby { get; init; }
    public DateTime createddate { get; init; }
}
