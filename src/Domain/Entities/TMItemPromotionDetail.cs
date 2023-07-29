using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("PromotionID", "ItemID")]
[Table("TMItemPromotionDetail")]
public partial class TMItemPromotionDetail
{
    [Key]
    public int PromotionID { get; set; }

    [Key]
    public int ItemID { get; set; }

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

    [ForeignKey("ItemID")]
    [InverseProperty("TMItemPromotionDetails")]
    public virtual TMItem Item { get; set; } = null!;

    [ForeignKey("PromotionID")]
    [InverseProperty("TMItemPromotionDetails")]
    public virtual TMItemPromotion Promotion { get; set; } = null!;
}
