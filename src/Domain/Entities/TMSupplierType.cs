using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplierType")]
public partial class TMSupplierType : BaseAuditableEntity
{
    [Key]
    public int SupplierTypeID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string SupplierTypeName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("SupplierType")]
    public virtual ICollection<TMSupplier> TMSuppliers { get; set; } = new List<TMSupplier>();
}
