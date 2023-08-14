using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplierDetail")]
public partial class TMSupplierDetail : BaseAuditableEntity
{
    [Key]
    public int SupplierDetailID { get; set; }

    public int SupplierID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ZipCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    [ForeignKey("SupplierID")]
    [InverseProperty("TMSupplierDetails")]
    public virtual TMSupplier Supplier { get; set; } = null!;
}
