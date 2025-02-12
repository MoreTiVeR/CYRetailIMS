using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
public class InquiryCountStockByBranchIDResponseDTO
{
    /// <summary>
    /// สาขา
    /// </summary>
    public int branchid { get; set; }

    /// <summary>
    /// รหัสสินค้า
    /// </summary>
    public int itemid { get; set; }

    /// <summary>
    /// ประเภท
    /// </summary>
    public string itemtypecode { get; set; }

    /// <summary>
    /// ประเภทย่อย
    /// </summary>
    public string subitemcode { get; set; }

    /// <summary>
    /// สต๊อกหน้าร้าน
    /// </summary>
    public int storestock { get; set; }

    /// <summary>
    /// ยอดนับได้
    /// </summary>
    public int countedqty { get; set; }

    /// <summary>
    /// รอเติม
    /// </summary>
    public int waitingtorestock { get; set; }

    /// <summary>
    /// ชำรุด
    /// </summary>
    public int damaged { get; set; }

    /// <summary>
    /// ขายก่อนนับ
    /// </summary>
    public int soldbeforecount { get; set; }

    /// <summary>
    /// รวมนับได้
    /// </summary>
    public int totalcounted { get; set; }
    //public int totalcounted => countedqty + waitingtorestock + damaged + soldbeforecount;

    /// <summary>
    /// ขาด/เกิน
    /// </summary>
    public int difference { get; set; }
    //public int difference => totalcounted - storestock;
}
