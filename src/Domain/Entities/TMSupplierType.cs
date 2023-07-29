using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplierType")]
public partial class TMSupplierType
{
    [Key]
    public int SupplierTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string SupplierTypeName { get; set; } = null!;

    [StringLength(10)]
    public string? Description { get; set; }

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

    [InverseProperty("SupplierType")]
    public virtual ICollection<TMSupplier> TMSuppliers { get; set; } = new List<TMSupplier>();
}
