using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;

[Serializable]
public class InventoryTransferReportByDraftIDResponseDTO
{
    public int transferheaderid { get; set; }
    public string refno { get; set; }
    public int sourcebranchid { get; set; }
    public string sourcebranchname { get; set; }
    public int destinationbranchid { get; set; }
    public string destinationbranchname { get; set; }
    public string description { get; set; }
    public string createdby { get; set; }
    public string createdbyname { get; set; }
    public DateTime createddate { get; set; }

    public List<InventoryTransferReportByDraftIDDetailDTO> detail { get; set; }
    public int totaltransferqty { get; set; }

    public List<InventoryTransferReportByDraftIDSubItemTypeDTO> subitemdetail { get; set; }
    public int totalsubitemtransferqty { get; set; }
}
