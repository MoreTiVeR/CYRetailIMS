using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStocks.v1;
public class InquiryCountStockResponseDTO
{
    public int countstockid { get; set; }
    public DateTime countstockdate { get; set; }
    public int branchid { get; set; }
    public string branchname { get; set; }

    public int countstockdetailid { get; set; }
    public int subitemtypeid { get; set; }
    public string subitemtypename { get; set; }

    /// <summary>
    /// สต๊อกจริงหน้าร้าน ณ วันนับ (คำนวนจากระบบ)
    /// </summary>
    public int qtyinbranchofcountstockday { get; set; }

    /// <summary>
    /// สต๊อกหน้าร้าน
    /// </summary>
    public int qtyinbranch { get; set; }

    /// <summary>
    /// สตีอกน้าร้าน(ยอดที่นับได้)
    /// </summary>
    public int countedamountqty { get; set; }

    /// <summary>
    /// รอเติม
    /// </summary>
    public int pendingrestockqty { get; set; }

    /// <summary>
    /// ชำรุด
    /// </summary>
    public int damagedqty { get; set; }

    /// <summary>
    /// ขายก่อนนับ
    /// </summary>
    public int salebeforecountqty { get; set; }

    /// <summary>
    /// ขาดเกินระบบ
    /// </summary>
    public int shortagesurplussystemqty => countedamountqty - qtyinbranchofcountstockday;

    /// <summary>
    /// ขาดเกินสต๊อกหน้าร้าน
    /// </summary>
    public int shortagesurplusqty => countedamountqty - qtyinbranch;

    /// <summary>
    /// รวมนับได้
    /// </summary>
    public int totalcount { get; set; }

    public string? remark { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }

}
