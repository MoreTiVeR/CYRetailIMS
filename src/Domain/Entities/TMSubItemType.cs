using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSubItemType")]
public partial class TMSubItemType : BaseAuditableEntity
{
    [Key]
    public int SubItemTypeID { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string SubItemCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string SubTypeNameTH { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string SubTypeNameEN { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("SubItemType")]
    public virtual ICollection<TMItem> TMItems { get; set; } = new List<TMItem>();

    [InverseProperty("SubItemType")]
    public virtual ICollection<TMSubItemTypeInItemType> TMSubItemTypeInItemTypes { get; set; } = new List<TMSubItemTypeInItemType>();
}
