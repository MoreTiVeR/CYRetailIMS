using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;

[Serializable]
public class InventoryTransferReportByDraftIDDetailDTO
{
    public int seq { get; set; }
    public int transferdetailid { get; set; }
    public int itemid { get; set; }
    public string itemcode { get; set; }
    public string itemname { get; set; }
    public int transferqty { get; set; }
    public int receiveqty { get; set; }
    public int excessqty { get; set; }
    public int? subitemtypeid { get; set; }
    public string? subitemtypename { get; set; }

}
