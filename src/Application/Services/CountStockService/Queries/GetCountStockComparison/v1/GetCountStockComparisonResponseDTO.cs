namespace CYRetailIMS.Application.Services.CountStockService.Queries.GetCountStockComparison.v1;

/// <summary>
/// DTO for หน้าเทียบข้อมูล - stock comparison per item/subitemtype
/// </summary>
public class GetCountStockComparisonResponseDTO
{
    public int itemid { get; set; }
    public string itemcode { get; set; }
    public string itemname { get; set; }
    public DateTime? comparedate { get; set; }
    public string subitemtypename { get; set; }
    public int subitemtypeid { get; set; }

    /// <summary>
    /// ยอดนับของหัวหน้า PC (HeadPC)
    /// </summary>
    public int? headpc_countedqty { get; set; }

    /// <summary>
    /// สต๊อกจริงในระบบ CY
    /// </summary>
    public int cy_stockqty { get; set; }

    /// <summary>
    /// ยอดขาย (ในช่วงวันที่เลือก)
    /// </summary>
    public int salesqty { get; set; }

    /// <summary>
    /// สินค้าเข้า
    /// </summary>
    public int stockinqty { get; set; }

    /// <summary>
    /// สินค้าออก
    /// </summary>
    public int stockoutqty { get; set; }

    /// <summary>
    /// ยอดนับได้จาก PC
    /// </summary>
    public int pc_countedqty { get; set; }

    /// <summary>
    /// ขาด/เกิน = ยอดนับได้ - สต๊อกระบบ
    /// </summary>
    public int shortagesurplusqty => pc_countedqty - cy_stockqty;
}
