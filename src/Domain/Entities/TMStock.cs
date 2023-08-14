using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMStock")]
public partial class TMStock : BaseAuditableEntity
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

    [ForeignKey("WarehouseID")]
    [InverseProperty("TMStocks")]
    public virtual TMWarehouse Warehouse { get; set; } = null!;
}
