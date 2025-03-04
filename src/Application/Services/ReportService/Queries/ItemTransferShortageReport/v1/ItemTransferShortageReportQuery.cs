using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemTransferShortageReport.v1;
public record ItemTransferShortageReportQuery : IRequest<BaseResponse<List<ItemTransferShortageReportResponseDTO>>>
{
    public DateTime? transferstartdate { get; init; }
    public DateTime? transferenddate { get; init; }
    public int? branchid { get; init; }
    public int? subitemtypeid { get; init; }
}
