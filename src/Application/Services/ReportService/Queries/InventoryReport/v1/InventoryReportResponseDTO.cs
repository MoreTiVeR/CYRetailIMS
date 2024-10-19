using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ReportService.Queries.InventoryReport.v1;

[Serializable]
public class InventoryReportResponseDTO
{
    public int itemid { get; set; }
    public string itemcode { get; set; }
    public string itemname { get; set; }
    public int qtyinstock { get; set; }
    public int totalsale { get; set; }
    public int notifymin { get; set; }
    public int notifymax { get; set; }

    private int _firstordernum { get; set; }
    /// <summary>
    /// สินค้าคงเหลิอคลัง ลบด้วย จำนวนขายทั้งหมด(ชิ้น)
    /// qtyinstock - totalsale
    /// </summary>
    public int firstordernum
    {
        get
        {
            var qtyorder1 = this.totalsale - this.qtyinstock;
            return qtyorder1 < 0 ? 0 : qtyorder1;
        }
        set
        {
            value = _firstordernum;
        }
    }

    private int _secoundordernum { get; set; }
    /// <summary>
    /// ขั้นต่ำคลัง ลบด้วย สินค้าคงเหลิอคลัง
    /// notifymin - qtyinstock
    /// </summary>
    public int secoundordernum
    {
        get
        {
            int qtyorder2 = this.notifymin - this.qtyinstock;
            return qtyorder2 < 0 ? 0 : qtyorder2;
        }
        set
        {
            value = _secoundordernum;
        }
    }
}
