using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.ItemStockReport.v1;
public class ItemStockReportDetailDTO
{
    public int branchid { get; set; }

    public string branchname { get; set; }

    public string itemcode { get; set; }

    public string itemname { get; set; }

    public int itemtypeid { get; set; }

    public string itemtypename { get; set; }

    public int? subitemtypeid { get; set; }

    public string? subitemtypename { get; set; }

    public int brandid { get; set; }

    public string brandname { get; set; }

    /// <summary>
    /// ราคาต้นทุน
    /// </summary>
    public decimal cost { get; set; }

    /// <summary>
    /// ราคาขาย
    /// </summary>
    public decimal price { get; set; }

    /// <summary>
    /// จำนวนสินค้าในสต็อก
    /// </summary>
    public int qty { get; set; }

    /// <summary>
    /// จำนวน QTY แจ้งเตือนขั้นต่ำ
    /// </summary>
    public int notifyminqty { get; set; }

    /// <summary>
    /// จำนวน QTY แจ้งเตือนสูงสุด
    /// </summary>
    public int? notifymaxqty { get; set; }

    public int refillamount
    {
        get
        {
            if (qty < notifyminqty)
            {
                return Math.Abs(qty - notifyminqty);
            }
            return 0;
        }
    }

    public bool isactive { get; set; }
}
