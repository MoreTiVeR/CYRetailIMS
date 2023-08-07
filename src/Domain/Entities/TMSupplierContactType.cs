using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplierContactType")]
public partial class TMSupplierContactType
{
    [Key]
    public int SupplierContactTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string SupplierContactTypeName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
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

    [InverseProperty("SupplierContactType")]
    public virtual ICollection<TMSupplierContact> TMSupplierContacts { get; set; } = new List<TMSupplierContact>();
}
