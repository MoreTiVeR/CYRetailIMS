using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMItemPromotion")]
public partial class TMItemPromotion
{
    [Key]
    public int PromotionID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PromotionName { get; set; } = null!;

    [Column(TypeName = "decimal(8, 4)")]
    public decimal PromotionPrice { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

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

    [InverseProperty("Promotion")]
    public virtual ICollection<TMItemPromotionDetail> TMItemPromotionDetails { get; set; } = new List<TMItemPromotionDetail>();
}
