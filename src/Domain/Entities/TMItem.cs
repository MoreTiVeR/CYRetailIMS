using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMItem")]
public partial class TMItem : BaseAuditableEntity
{
    [Key]
    public int ItemID { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string ItemCode { get; set; } = null!;

    public int ItemTypeID { get; set; }

    public int BrandID { get; set; }

    public int UnitOfMeasureID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? ShortName { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(13)]
    [Unicode(false)]
    public string? BarCode { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Cost { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    public double DiscountPercent { get; set; }

    public int Qty { get; set; }

    public int NotifyMinQty { get; set; }

    [Unicode(false)]
    public string? ItemImageUrl { get; set; }

    [ForeignKey("BrandID")]
    [InverseProperty("TMItems")]
    public virtual TMItemBrand Brand { get; set; } = null!;

    [ForeignKey("ItemTypeID")]
    [InverseProperty("TMItems")]
    public virtual TMItemType ItemType { get; set; } = null!;

    [InverseProperty("Item")]
    public virtual ICollection<TMItemInBranch> TMItemInBranches { get; set; } = new List<TMItemInBranch>();

    [InverseProperty("Item")]
    public virtual ICollection<TMItemPromotionDetail> TMItemPromotionDetails { get; set; } = new List<TMItemPromotionDetail>();

    [InverseProperty("Item")]
    public virtual ICollection<TTStockTransaction> TTStockTransactions { get; set; } = new List<TTStockTransaction>();

    [ForeignKey("UnitOfMeasureID")]
    [InverseProperty("TMItems")]
    public virtual TMUnitOfMeasure UnitOfMeasure { get; set; } = null!;
}
