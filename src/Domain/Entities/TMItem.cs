using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMItem")]
public partial class TMItem
{
    [Key]
    public int ItemID { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string ItemCode { get; set; } = null!;

    public int ItemTypeID { get; set; }

    public int UnitOfMeasureID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(5)]
    [Unicode(false)]
    public string? ShortName { get; set; }

    [StringLength(13)]
    [Unicode(false)]
    public string? BarCode { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Price { get; set; }

    [Unicode(false)]
    public string? ItemImageUrl { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreadedDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [Required]
    public bool? Status { get; set; }

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
