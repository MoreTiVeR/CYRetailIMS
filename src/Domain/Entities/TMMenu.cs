using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TMMenu
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
    public string? CMS_href { get; set; }

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

    [InverseProperty("Menu")]
    public virtual ICollection<TMRoleInMenu> TMRoleInMenus { get; set; } = new List<TMRoleInMenu>();
}
