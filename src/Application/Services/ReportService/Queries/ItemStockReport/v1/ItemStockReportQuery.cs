using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using MediatR;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
public record ItemStockReportQuery : IRequest<BaseResponse<ItemStockReportResponseDTO>>
{
    /// <summary>
    /// Brach ID to filter the report by a specific branch.
    /// </summary>
    public int? branchid { get; init; }

    /// <summary>
    /// Item type ID to filter the report by a specific item type.
    /// </summary>
    public int? itemtypeid { get; init; }

    /// <summary>
    /// Subitem type ID to filter the report by a specific subitem type.
    /// </summary>
    public int? subitemtypeid { get; init; }

    public int? brandid { get; set; }

    /// <summary>
    /// Start index 1
    /// </summary>
    public int startrow { get; set; }

    /// <summary>
    /// Total row take
    /// </summary>
    public int pagesize { get; set; }

    public string? searchvalue { get; set; }

    public bool isexportalldata { get; set; }
}
