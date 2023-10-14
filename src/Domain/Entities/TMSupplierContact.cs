using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplierContact")]
public partial class TMSupplierContact : BaseAuditableEntity
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

	[StringLength(13)]
	[Unicode(false)]
	public string? MobileNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    [ForeignKey("SupplierID")]
    [InverseProperty("TMSupplierContacts")]
    public virtual TMSupplier Supplier { get; set; } = null!;

    [ForeignKey("SupplierContactTypeID")]
    [InverseProperty("TMSupplierContacts")]
    public virtual TMSupplierContactType SupplierContactType { get; set; } = null!;
}
