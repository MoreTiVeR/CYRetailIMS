using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTCountStockDetail")]
public partial class TTCountStockDetail : BaseAuditableEntity
{
    [Key]
    public int CountStockDetailID { get; set; }

    /// <summary>
    /// สินค้าที่ทำการนับ (รองรับหน้านับสต๊อกแบบรายสินค้า)
    /// </summary>
    public int? ItemID { get; set; }

    public int SubItemTypeID { get; set; }

    public int CountStockID { get; set; }

    public int QtyInBranchOfCountStockDay { get; set; }

    public int QtyInBranch { get; set; }

    public int CountedAmountQty { get; set; }

    public int PendingReStockQty { get; set; }

    public int DamagedQty { get; set; }

    public int SaleBeforeCountQty { get; set; }

    public int TotalCountQty { get; set; }

    public int ShortageSurplusQty { get; set; }

    /// <summary>
    /// หมายเหตุรายการ (กรณีนับได้ 0 ต้องระบุเหตุผล)
    /// </summary>
    [StringLength(200)]
    [Unicode(false)]
    public string? ItemRemark { get; set; }

    [ForeignKey("CountStockID")]
    [InverseProperty("TTCountStockDetails")]
    public virtual TTCountStock CountStock { get; set; } = null!;
}
