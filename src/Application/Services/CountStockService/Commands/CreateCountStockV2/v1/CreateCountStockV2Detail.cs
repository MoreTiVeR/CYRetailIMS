namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStockV2.v1;

public record CreateCountStockV2Detail
{
    /// <summary>รหัสสินค้า (ItemID)</summary>
    public int itemid { get; init; }

    /// <summary>รหัสประเภทย่อย (0 ถ้าไม่มี)</summary>
    public int subitemtypeid { get; init; }

    /// <summary>สต๊อกในระบบ ณ วันนับ</summary>
    public int qtyinbranchofcountstockday { get; init; }

    /// <summary>จำนวนที่มีอยู่จริง (user input)</summary>
    public int physicalcountqty { get; init; }

    /// <summary>ขาด/เกิน — computed, อาจติดลบได้</summary>
    public int shortagesurplusqty => physicalcountqty - qtyinbranchofcountstockday;
}
