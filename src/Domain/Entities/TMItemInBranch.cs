using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("BranchID", "ItemID")]
[Table("TMItemInBranch")]
public partial class TMItemInBranch
{
    [Key]
    public int BranchID { get; set; }

    [Key]
    public int ItemID { get; set; }

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
    [InverseProperty("TMItemInBranches")]
    public virtual TMBranch Branch { get; set; } = null!;

    [ForeignKey("ItemID")]
    [InverseProperty("TMItemInBranches")]
    public virtual TMItem Item { get; set; } = null!;
}
