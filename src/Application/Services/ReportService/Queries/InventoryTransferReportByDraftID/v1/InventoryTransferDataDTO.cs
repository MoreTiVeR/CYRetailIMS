using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;

[Serializable]
public class InventoryTransferDataDTO
{
    public int TransferHeaderID { get; set; }
    public string TransferRefNo { get; set; }
    public int TransferDetailID { get; set; }
    public int SourceBranchID { get; set; }
    public string SourceBranchName { get; set; }
    public string Description { get; set; }
    public int BranchID { get; set; }
    public string BranchName { get; set; }
    public int ItemID { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public int SubItemTypeID { get; set; }
    public string SubItemTypeName { get; set; }
    public int Qty { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }

}
