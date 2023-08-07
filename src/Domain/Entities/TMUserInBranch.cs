using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("UserID", "BranchID")]
public partial class TMUserInBranch : BaseAuditableEntity
{
    [Key]
    public int UserID { get; set; }

    [Key]
    public int BranchID { get; set; }

    [ForeignKey("BranchID")]
    [InverseProperty("TMUserInBranches")]
    public virtual TMBranch Branch { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("TMUserInBranches")]
    public virtual TMUser User { get; set; } = null!;
}
