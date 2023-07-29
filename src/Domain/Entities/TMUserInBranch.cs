using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("UserID", "BranchID")]
public partial class TMUserInBranch
{
    [Key]
    public int UserID { get; set; }

    [Key]
    public int BranchID { get; set; }

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

    [ForeignKey("BranchID")]
    [InverseProperty("TMUserInBranches")]
    public virtual TMBranch Branch { get; set; } = null!;

    [ForeignKey("UserID")]
    [InverseProperty("TMUserInBranches")]
    public virtual TMUser User { get; set; } = null!;
}
