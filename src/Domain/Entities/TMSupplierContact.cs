using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplierContact")]
public partial class TMSupplierContact
{
    [Key]
    public int SupplierContactID { get; set; }

    public int SupplierID { get; set; }

    public int SupplierContactTypeID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ContactAccountName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? ContactPerson { get; set; }

    [StringLength(100)]
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

    [ForeignKey("SupplierID")]
    [InverseProperty("TMSupplierContacts")]
    public virtual TMSupplier Supplier { get; set; } = null!;

    [ForeignKey("SupplierContactTypeID")]
    [InverseProperty("TMSupplierContacts")]
    public virtual TMSupplierContactType SupplierContactType { get; set; } = null!;
}
