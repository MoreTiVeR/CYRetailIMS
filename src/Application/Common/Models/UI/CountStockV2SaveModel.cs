using System.Collections.Generic;

namespace CYRetailIMS.Application.Common.Models.UI;

/// <summary>
/// Model รับข้อมูลบันทึกนับสต๊อก V2 (ระบุแค่จำนวนที่มีอยู่จริง)
/// </summary>
public class CountStockV2SaveModel
{
    public int BranchID { get; set; }
    public string? Remark { get; set; }
    public List<CountStockV2ItemModel> Items { get; set; } = new();
}

public class CountStockV2ItemModel
{
    /// <summary>รหัสสินค้า</summary>
    public int ItemID { get; set; }

    /// <summary>รหัสประเภทย่อย (จาก TMItem.SubItemTypeID, อาจเป็น null)</summary>
    public int? SubItemTypeID { get; set; }

    /// <summary>สต๊อกในระบบ ณ วันนับ</summary>
    public int QtyInBranchOfStockDay { get; set; }

    /// <summary>จำนวนที่มีอยู่จริง (พนง ระบุ)</summary>
    public int PhysicalCountQty { get; set; }
}
