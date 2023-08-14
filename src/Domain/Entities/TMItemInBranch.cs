using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("BranchID", "ItemID")]
[Table("TMItemInBranch")]
public partial class TMItemInBranch : BaseAuditableEntity
{
    [Key]
    public int BranchID { get; set; }

    [Key]
    public int ItemID { get; set; }

    [ForeignKey("BranchID")]
    [InverseProperty("TMItemInBranches")]
    public virtual TMBranch Branch { get; set; } = null!;

    [ForeignKey("ItemID")]
    [InverseProperty("TMItemInBranches")]
    public virtual TMItem Item { get; set; } = null!;
}
