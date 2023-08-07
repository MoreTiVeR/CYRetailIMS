using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTPurchaseOrder")]
public partial class TTPurchaseOrder
{
    [Key]
    public int PurchaseOrderID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PurchaseOrderName { get; set; } = null!;

    public int PurchaseTypeID { get; set; }

    public int SupplierID { get; set; }

    public int CurrencyID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceivedDate { get; set; }

    public int PaymenTypeID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Tax { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Total { get; set; }

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

    public int ApproveStatus { get; set; }

    [ForeignKey("CurrencyID")]
    [InverseProperty("TTPurchaseOrders")]
    public virtual TMCurrency Currency { get; set; } = null!;

    [ForeignKey("PaymenTypeID")]
    [InverseProperty("TTPurchaseOrders")]
    public virtual TMPaymentType PaymenType { get; set; } = null!;

    [ForeignKey("PurchaseTypeID")]
    [InverseProperty("TTPurchaseOrders")]
    public virtual TMPurchaseType PurchaseType { get; set; } = null!;

    [ForeignKey("SupplierID")]
    [InverseProperty("TTPurchaseOrders")]
    public virtual TMSupplier Supplier { get; set; } = null!;

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<TTPurchaseOrderDetail> TTPurchaseOrderDetails { get; set; } = new List<TTPurchaseOrderDetail>();

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<TTShipment> TTShipments { get; set; } = new List<TTShipment>();
}
