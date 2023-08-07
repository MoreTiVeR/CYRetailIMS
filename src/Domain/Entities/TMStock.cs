using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMStock")]
public partial class TMStock
{
    [Key]
    public int StockID { get; set; }

    public int ItemID { get; set; }

    public int WarehouseID { get; set; }

    public int QtyInStock { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal TotalPrice { get; set; }

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

    [ForeignKey("WarehouseID")]
    [InverseProperty("TMStocks")]
    public virtual TMWarehouse Warehouse { get; set; } = null!;
}
