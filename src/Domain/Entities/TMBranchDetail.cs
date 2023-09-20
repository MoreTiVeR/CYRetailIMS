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

    [StringLength(200)]
    [Unicode(false)]
    public string Address1 { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? Address2 { get; set; }

    public int? SubDistrictID { get; set; }

    public int? DistrictID { get; set; }

    public int? ProvinceID { get; set; }

    public int? ZipCode { get; set; }

    [ForeignKey("BranchID")]
    [InverseProperty("TMBranchDetail")]
    public virtual TMBranch Branch { get; set; } = null!;
}
