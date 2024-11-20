using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInventoryForTransferByBranchID.v1;
using CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;
using MediatR;

namespace CYRetailIMS.Application.Services.ExcelService.Queries.GenerateStockTransferExcelReport.v1;
public record GenerateStockTransferExcelReportQuery : IRequest<BaseResponse<GenerateStockTransferExcelReportResponseDTO>>
{
    public int draftid { get; init; }
    public InventoryTransferReportByDraftIDResponseDTO reportdata { get; init; }
}
