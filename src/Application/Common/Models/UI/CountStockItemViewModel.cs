using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CountStockItemViewModel
{
    /// <summary>
    /// สาขา
    /// </summary>
    public int BranchID { get; set; }

    /// <summary>
    /// รหัสสินค้า
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// ประเภท
    /// </summary>
    public string ItemTypeCode { get; set; }

    /// <summary>
    /// รหัสประเภทย่อย
    /// </summary>
    public int SubItemTypeID { get; set; }

    /// <summary>
    /// ประเภทย่อย
    /// </summary>
    public string SubItemCode { get; set; }

    /// <summary>
    /// สต๊อกหน้าร้าน ณ วันที่นับสต๊อก (ระบบดึงให้)
    /// </summary>
    public int QtyInBranchOfStockDay { get; set; }

    /// <summary>
    /// สต๊อกหน้าร้าน (พนง ระบุเอง)
    /// </summary>
    public int StoreStock { get; set; }

    /// <summary>
    /// ยอดนับได้ (พนง ระบุเอง)
    /// </summary>
    public int CountedQty { get; set; }

    /// <summary>
    /// รอเติม (พนง ระบุเอง)
    /// </summary>
    public int WaitingToRestock { get; set; }

    /// <summary>
    /// ชำรุด (พนง ระบุเอง)
    /// </summary>
    public int Damaged { get; set; }

    /// <summary>
    /// ขายก่อนนับ (พนง ระบุเอง)
    /// </summary>
    public int SoldBeforeCount { get; set; }

    /// <summary>
    /// รวมนับได้
    /// </summary>
    public int TotalCounted { get; set; }

    /// <summary>
    /// ขาด/เกิน
    /// </summary>
    public int Difference { get; set; }
}
