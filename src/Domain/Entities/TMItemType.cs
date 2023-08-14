using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMItemType")]
public partial class TMItemType : BaseAuditableEntity
{
    [Key]
    public int ItemTypeID { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string ItemTypeName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("ItemType")]
    public virtual ICollection<TMItem> TMItems { get; set; } = new List<TMItem>();
}
