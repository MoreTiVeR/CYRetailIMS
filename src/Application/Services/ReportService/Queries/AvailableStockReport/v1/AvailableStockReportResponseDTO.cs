using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockReport.v1;

[Serializable]
public class AvailableStockReportResponseDTO
{
    public int itemid { get; set; }
    public string itemname { get; set; }
    public int itemcode { get; set; }
    public int itemtypeid { get; set; }
    public string itemtypename { get; set; }
    public string barcode { get; set; }
    public decimal price { get; set; }
    public int qty { get; set; }
    public int minqty { get; set; }
    public int maxqty { get; set; }
    public int branchid { get; set; }
    public int branchname { get; set; }
}
