using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTPurchaseOrder")]
public partial class TTPurchaseOrder : BaseAuditableEntity
{
    [Key]
    public int PurchaseOrderID { get; set; }

    [StringLength(18)]
    [Unicode(false)]
	public string PurchaseOrderNo { get; set; } = null!;

	public int PurchaseTypeID { get; set; }

    public int SupplierID { get; set; }

    public int CurrencyID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReceivedDate { get; set; }

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
