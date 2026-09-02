using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTCountStockApprovalHistory")]
public partial class TTCountStockApprovalHistory : BaseAuditableEntity
{
    [Key]
    public int CountStockApprovalHistoryID { get; set; }

    public int CountStockID { get; set; }

    public int CountStockDetailID { get; set; }

    public int BranchID { get; set; }

    public int ItemID { get; set; }

    public int SubItemTypeID { get; set; }

    public int QtyInBranchOfCountStockDay { get; set; }

    public int QtyInBranchBeforeApprove { get; set; }

    public int QtyInBranchAfterApprove { get; set; }

    public int CountedAmountQty { get; set; }

    public int PendingReStockQty { get; set; }

    public int DamagedQty { get; set; }

    public int SaleBeforeCountQty { get; set; }

    public int TotalCountQty { get; set; }

    public int ShortageSurplusQty { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ItemRemark { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string CounterRole { get; set; } = "PC";

    [StringLength(10)]
    [Unicode(false)]
    public string ApprovedBy { get; set; } = string.Empty;

    [Column(TypeName = "datetime")]
    public DateTime CountStockDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ApprovedDate { get; set; }
}
