using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTPurchaseOrderDetail")]
public partial class TTPurchaseOrderDetail
{
    [Key]
    public int PurchaseOrderDetailID { get; set; }

    public int PurchaseOrderID { get; set; }

    public int ItemID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal DiscountPercentage { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal TaxPercentage { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal TaxAmount { get; set; }

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

    [ForeignKey("PurchaseOrderID")]
    [InverseProperty("TTPurchaseOrderDetails")]
    public virtual TTPurchaseOrder PurchaseOrder { get; set; } = null!;
}
