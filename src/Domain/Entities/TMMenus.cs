using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TMMenus : BaseAuditableEntity
{
    [Key]
    public int MenuID { get; set; }

    public int Seq { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string MenuName_TH { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string MenuName_EN { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CMS_DataIconName { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? CMS_Link { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CMS_Title { get; set; }

    [InverseProperty("Menu")]
    public virtual ICollection<TMRoleInMenu> TMRoleInMenus { get; set; } = new List<TMRoleInMenu>();
}
