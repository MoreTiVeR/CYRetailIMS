using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMItemPromotion")]
public partial class TMItemPromotion : BaseAuditableEntity
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

    [InverseProperty("Promotion")]
    public virtual ICollection<TMItemPromotionDetail> TMItemPromotionDetails { get; set; } = new List<TMItemPromotionDetail>();
}
