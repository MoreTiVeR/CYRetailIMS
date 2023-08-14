using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMBranchDetail")]
public partial class TMBranchDetail : BaseAuditableEntity
{
    [Key]
    public int BranchID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Address1 { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Address2 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string SubDistrictCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string DistrictCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ProvinceCode { get; set; } = null!;

    [StringLength(5)]
    [Unicode(false)]
    public string ZipCode { get; set; } = null!;

    [ForeignKey("BranchID")]
    [InverseProperty("TMBranchDetail")]
    public virtual TMBranch Branch { get; set; } = null!;
}
