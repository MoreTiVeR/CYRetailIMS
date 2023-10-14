using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplier")]
public partial class TMSupplier : BaseAuditableEntity
{
    [Key]
    public int SupplierID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string SupplierName_TH { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string SupplierName_EN { get; set; } = null!;

    public int SupplierTypeID { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }

    [ForeignKey("SupplierTypeID")]
    [InverseProperty("TMSuppliers")]
    public virtual TMSupplierType SupplierType { get; set; } = null!;

    [InverseProperty("Supplier")]
    public virtual ICollection<TMSupplierContact> TMSupplierContacts { get; set; } = new List<TMSupplierContact>();

    [InverseProperty("Supplier")]
    public virtual ICollection<TMSupplierDetail> TMSupplierDetails { get; set; } = new List<TMSupplierDetail>();

    [InverseProperty("Supplier")]
    public virtual ICollection<TTPurchaseOrder> TTPurchaseOrders { get; set; } = new List<TTPurchaseOrder>();
}
