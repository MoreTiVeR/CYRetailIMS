namespace CYRetailIMS.Application.Common.Models.UI;

/// <summary>
/// ViewModel for new count stock entry (หน้านับสต๊อกแบบใหม่)
/// Used for both PC and HeadPC roles
/// </summary>
public class NewCountStockEntryModel
{
    public int BranchID { get; set; }
    public string ItemTypeCode { get; set; }
    public int SubItemTypeID { get; set; }
    public string SubItemCode { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public int ItemId { get; set; }

    /// <summary>
    /// สต๊อกจริง CY (ดึงอัตโนมัติจากระบบ)
    /// </summary>
    public int CYStockQty { get; set; }

    /// <summary>
    /// ยอดนับได้ (พนง กรอก)
    /// </summary>
    public int CountedQty { get; set; }

    /// <summary>
    /// รอเติม
    /// </summary>
    public int WaitingToRestock { get; set; }

    /// <summary>
    /// ชำรุด
    /// </summary>
    public int Damaged { get; set; }

    /// <summary>
    /// ขายก่อนนับ
    /// </summary>
    public int SoldBeforeCount { get; set; }

    /// <summary>
    /// รวมนับได้ = CountedQty + WaitingToRestock + Damaged + SoldBeforeCount (E+F+G+H)
    /// ไม่รวมสต๊อกระบบ (CYStockQty) เพื่อไม่ให้นับซ้ำ — ดูหมายเหตุใน countstock_newentry.js
    /// </summary>
    public int TotalCounted { get; set; }

    /// <summary>
    /// ขาด/เกิน = CountedQty - CYStockQty
    /// </summary>
    public int Difference { get; set; }

    /// <summary>
    /// หมายเหตุรายการ (บังคับกรอกถ้ายอดนับได้ = 0)
    /// </summary>
    public string? ItemRemark { get; set; }

    /// <summary>
    /// หมายเหตุรวม
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// บทบาทผู้นับ: "PC" หรือ "HeadPC"
    /// </summary>
    public string CounterRole { get; set; }

    /// <summary>
    /// true = บันทึกเฉพาะรายการที่ถูกกรอง/ค้นหา (partial merge), false = บันทึกทั้งชุด (full replace)
    /// </summary>
    public bool IsPartialSave { get; set; }
}
