using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMAdjustItemType")]
public partial class TMAdjustItemType : BaseAuditableEntity
{
    [Key]
    public int AdjustTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AdjustTypeName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }
}
