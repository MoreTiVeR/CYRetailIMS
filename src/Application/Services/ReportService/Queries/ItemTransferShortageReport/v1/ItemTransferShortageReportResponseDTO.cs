using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemTransferShortageReport.v1;
public class ItemTransferShortageReportResponseDTO
{
    public int transferhistoryid { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }
    public int itemid { get; set; }
    public string itemcode { get; set; }
    public string itemname { get; set; }
    public int itemtypeid { get; set; }
    public string itemtypename { get; set; }

    public int? subitemtypeid { get; set; }
    public string? subitemtypename { get; set; }

    public int brandid { get; set; }
    public string brandname { get; set; }

    /// <summary>
    /// จำนวนที่ต้องเติม
    /// </summary>
    public int suggestrefillqtybysystem { get; set; }

    /// <summary>
    /// จำนวนที่เติม
    /// </summary>
    public int refillqty { get; set; }

    /// <summary>
    /// จำนวนที่ขาด
    /// </summary>
    public int shortageqty => suggestrefillqtybysystem - refillqty;

    public DateTime createddate { get; set; }
}
