using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
public record CreateCountStockDetail
{
    public int subitemtypeid { get; init; }

    /// <summary>
    /// สตีอกหน้าร้าน สาขา
    /// </summary>
    public int qtyinbranch { get; init; }

    /// <summary>
    /// ยอดนับได้
    /// </summary>
    public int countedamountqty { get; init; }

    /// <summary>
    /// รอเติม
    /// </summary>
    public int pendingrestockqty { get; init; }

    /// <summary>
    /// ชำรุด
    /// </summary>
    public int damagedqty { get; init; }

    /// <summary>
    /// ขายก่อนนับ
    /// </summary>
    public int salebeforecountqty { get; init; }

    /// <summary>
    /// รวมนับได้
    /// </summary>
    public int totalcountqty => countedamountqty + pendingrestockqty + damagedqty + salebeforecountqty;

    /// <summary>
    /// ขาดเกิน
    /// </summary>
    public int shortagesurplusqty => totalcountqty - qtyinbranch;
}
