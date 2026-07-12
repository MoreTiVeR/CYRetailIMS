namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryItemsInBranchV2.v1;

public class InquiryItemsInBranchV2ResponseDTO
{
    /// <summary>รหัสสาขา</summary>
    public int branchid { get; set; }

    /// <summary>รหัสสินค้า</summary>
    public int itemid { get; set; }

    /// <summary>รหัส (Code) สินค้า</summary>
    public string itemcode { get; set; }

    /// <summary>ชื่อสินค้า</summary>
    public string itemname { get; set; }

    /// <summary>รหัสประเภทย่อย (nullable — บางสินค้าอาจไม่มี)</summary>
    public int? subitemtypeid { get; set; }

    /// <summary>ชื่อประเภทย่อย</summary>
    public string subitemcode { get; set; }

    /// <summary>สต๊อกสาขา ณ วันที่นับ (ดึงจาก TMItemInBranch.Qty)</summary>
    public int qtyinbranch { get; set; }
}
