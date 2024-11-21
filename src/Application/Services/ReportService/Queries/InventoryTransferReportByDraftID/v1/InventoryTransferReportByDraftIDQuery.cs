using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferByDraftID.v1;
public record InventoryTransferReportByDraftIDQuery : IRequest<BaseResponse<InventoryTransferReportByDraftIDResponseDTO>>
{
    public int transferid { get; init; }
}
