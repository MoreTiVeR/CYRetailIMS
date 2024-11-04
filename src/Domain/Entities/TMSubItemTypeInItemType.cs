using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("ItemTypeID", "SubItemTypeID")]
[Table("TMSubItemTypeInItemType")]
public partial class TMSubItemTypeInItemType : BaseAuditableEntity
{
    [Key]
    public int ItemTypeID { get; set; }

    [Key]
    public int SubItemTypeID { get; set; }

    [ForeignKey("ItemTypeID")]
    [InverseProperty("TMSubItemTypeInItemTypes")]
    public virtual TMItemType ItemType { get; set; } = null!;

    [ForeignKey("SubItemTypeID")]
    [InverseProperty("TMSubItemTypeInItemTypes")]
    public virtual TMSubItemType SubItemType { get; set; } = null!;
}
