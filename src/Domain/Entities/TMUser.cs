using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TMUser
{
    [Key]
    public int UserID { get; set; }

    public int RoleID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [MaxLength(16)]
    public byte[] Password { get; set; } = null!;

    [StringLength(64)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string NickName { get; set; } = null!;

    [Unicode(false)]
    public string? ProfilePicture { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogout { get; set; }

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

    public int? ApproveStatus { get; set; }

    [ForeignKey("RoleID")]
    [InverseProperty("TMUsers")]
    public virtual TMRole Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<TMUserInBranch> TMUserInBranches { get; set; } = new List<TMUserInBranch>();
}
