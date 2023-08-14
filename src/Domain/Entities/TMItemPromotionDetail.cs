using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("PromotionID", "ItemID")]
[Table("TMItemPromotionDetail")]
public partial class TMItemPromotionDetail : BaseAuditableEntity
{
    [Key]
    public int PromotionID { get; set; }

    [Key]
    public int ItemID { get; set; }

    [ForeignKey("ItemID")]
    [InverseProperty("TMItemPromotionDetails")]
    public virtual TMItem Item { get; set; } = null!;

    [ForeignKey("PromotionID")]
    [InverseProperty("TMItemPromotionDetails")]
    public virtual TMItemPromotion Promotion { get; set; } = null!;
}
