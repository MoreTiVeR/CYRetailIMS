using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMBranch")]
public partial class TMBranch
{
    [Key]
    public int BranchID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BranchCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string BranchName { get; set; } = null!;

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

    [InverseProperty("Branch")]
    public virtual TMBranchDetail? TMBranchDetail { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<TMItemInBranch> TMItemInBranches { get; set; } = new List<TMItemInBranch>();

    [InverseProperty("Branch")]
    public virtual ICollection<TMUserInBranch> TMUserInBranches { get; set; } = new List<TMUserInBranch>();

    [InverseProperty("Branch")]
    public virtual ICollection<TMWarehouse> TMWarehouses { get; set; } = new List<TMWarehouse>();
}
