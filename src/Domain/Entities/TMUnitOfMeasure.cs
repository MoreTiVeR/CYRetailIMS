using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMUnitOfMeasure")]
public partial class TMUnitOfMeasure : BaseAuditableEntity
{
    [Key]
    public int UnitOfMeasureID { get; set; }

    /// <summary>
    /// หน่วยวัด เช่น ชิ้น อัน กล่อง
    /// </summary>
    [StringLength(50)]
    [Unicode(false)]
    public string? UnitOfMeasureName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("UnitOfMeasure")]
    public virtual ICollection<TMItem> TMItems { get; set; } = new List<TMItem>();
}
