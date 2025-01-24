using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTCountStocksHistory")]
public partial class TTCountStocksHistory : BaseAuditableEntity
{
    [Key]
    public int CountStockHistoryID { get; set; }

    public int BranchID { get; set; }

    public int ItemID { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    public double DiscountPercent { get; set; }

    public int Qty { get; set; }

    public int? NotifyMinQty { get; set; }

    public int? NotifyMaxQty { get; set; }

    public int WarehouseQty { get; set; }

}
