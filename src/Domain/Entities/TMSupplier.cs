using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMSupplier")]
public partial class TMSupplier
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
