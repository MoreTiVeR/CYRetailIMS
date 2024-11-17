using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryTransferReportByDraftID.v1;
public class InventoryTransferReportByDraftIDSubItemTypeDTO
{
    public int seq { get; set; }
    public int subitemtypeid { get; set; }
    public string subitemtypename { get; set; }
    public int transferqty { get; set; }

}
