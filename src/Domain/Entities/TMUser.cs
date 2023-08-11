using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMUsers")]
public partial class TMUsers : BaseAuditableEntity
{
    [Key]
    public int UserID { get; set; }

    public int RoleID { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [MaxLength(16)]
    public byte[] Password { get; set; } = null!;

    [Unicode(false)]
    public string? ProfilePicture { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogout { get; set; }

    public int? ApproveStatus { get; set; }

    [ForeignKey("RoleID")]
    [InverseProperty("TMUsers")]
    public virtual TMRole Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<TMEmployee> TMEmployees { get; set; } = new List<TMEmployee>();

    [InverseProperty("User")]
    public virtual ICollection<TMUserInBranch> TMUserInBranches { get; set; } = new List<TMUserInBranch>();
}
