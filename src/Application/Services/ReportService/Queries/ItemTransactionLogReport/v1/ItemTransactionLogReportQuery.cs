using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemTransactionLogReport.v1;
public record ItemTransactionLogReportQuery : IRequest<BaseResponse<List<ItemTransactionLogReportResponseDTO>>>
{
    public int? branchid { get; init; }
}
