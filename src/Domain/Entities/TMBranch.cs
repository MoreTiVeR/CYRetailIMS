using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMBranch")]
public partial class TMBranch : BaseAuditableEntity
{
    [Key]
    public int BranchID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BranchCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string BranchName { get; set; } = null!;

    [InverseProperty("Branch")]
    public virtual TMBranchDetail? TMBranchDetail { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<TMItemInBranch> TMItemInBranches { get; set; } = new List<TMItemInBranch>();

    [InverseProperty("Branch")]
    public virtual ICollection<TMUserInBranch> TMUserInBranches { get; set; } = new List<TMUserInBranch>();
}
