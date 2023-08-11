using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TMRole: BaseAuditableEntity
{
    [Key]
    public int RoleID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<TMRoleInMenus> TMRoleInMenus { get; set; } = new List<TMRoleInMenus>();

    [InverseProperty("Role")]
    public virtual ICollection<TMUsers> TMUsers { get; set; } = new List<TMUsers>();
}
