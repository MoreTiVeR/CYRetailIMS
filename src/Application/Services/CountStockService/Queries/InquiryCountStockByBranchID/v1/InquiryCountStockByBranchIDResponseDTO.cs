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
    /// รหัสสินค้า (id)
    /// </summary>
    public int itemid { get; set; }

    /// <summary>
    /// รหัสสินค้า (code) — คอลัมน์ A ของหน้านับสต๊อก
    /// </summary>
    public string itemcode { get; set; }

    /// <summary>
    /// ชื่อสินค้า — คอลัมน์ B ของหน้านับสต๊อก
    /// </summary>
    public string itemname { get; set; }

    /// <summary>
    /// ประเภท
    /// </summary>
    public string itemtypecode { get; set; }


    /// <summary>
    /// รหัสประเภทย่อย
    /// </summary>
    public int subitemtypeid { get; set; }

    /// <summary>
    /// ประเภทย่อย
    /// </summary>
    public string subitemcode { get; set; }

    /// <summary>
    /// สต๊อกหน้าร้าน ณ วันที่นับสต๊อก (ระบบดึงให้)
    /// </summary>
    public int qtyinbranchofstockday { get; set; }

    /// <summary>
    /// สต๊อกหน้าร้าน (พนง ระบุเอง)
    /// </summary>
    public int storestock { get; set; }

    /// <summary>
    /// ยอดนับได้ (พนง ระบุเอง)
    /// </summary>
    public int countedqty { get; set; }

    /// <summary>
    /// รอเติม (พนง ระบุเอง)
    /// </summary>
    public int waitingtorestock { get; set; }

    /// <summary>
    /// ชำรุด (พนง ระบุเอง)
    /// </summary>
    public int damaged { get; set; }

    /// <summary>
    /// ขายก่อนนับ (พนง ระบุเอง)
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
