using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSubMenu")]
public partial class TMSubMenu
{
    [Key]
    public int SubMenuID { get; set; }

    public int MenuID { get; set; }

    public int Seq { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string MenuName_EN { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string MenuName_TH { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? CMS_ControllerName { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? CMS_ActionName { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? CMS_I_Class { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? CMS_Span_Class { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? CMS_Link { get; set; }

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

    [InverseProperty("SubMenu")]
    public virtual ICollection<TMRoleInMenu> TMRoleInMenus { get; set; } = new List<TMRoleInMenu>();
}
