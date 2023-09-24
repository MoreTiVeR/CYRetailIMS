using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[PrimaryKey("RoleID", "MenuID", "SubMenuID")]
public partial class TMRoleInMenu : BaseAuditableEntity
{
    [Key]
    public int RoleID { get; set; }

    [Key]
    public int MenuID { get; set; }

    [Key]
    public int SubMenuID { get; set; }

    public bool CanView { get; set; }

    public bool CanCreate { get; set; }

    public bool CanEdit { get; set; }

    public bool CanDelete { get; set; }

    [ForeignKey("MenuID")]
    [InverseProperty("TMRoleInMenus")]
    public virtual TMMenus Menu { get; set; } = null!;

    [ForeignKey("RoleID")]
    [InverseProperty("TMRoleInMenus")]
    public virtual TMRole Role { get; set; } = null!;

    [ForeignKey("SubMenuID")]
    [InverseProperty("TMRoleInMenus")]
    public virtual TMSubMenus SubMenu { get; set; } = null!;
}
