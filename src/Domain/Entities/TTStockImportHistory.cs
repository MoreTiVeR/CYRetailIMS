using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTStockImportHistory")]
public partial class TTStockImportHistory : BaseEntity
{
    [Key]
    public int ImportHistortID { get; set; }

    public int BranchID { get; set; }

    public int ItemID { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    public double DiscountPercent { get; set; }

    public int Qty { get; set; }

    public int? NotifyMinQty { get; set; }

    public int? NotifyMaxQty { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string ImportedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ImportedDate { get; set; }
}
