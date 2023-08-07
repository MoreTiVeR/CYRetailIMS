using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTStockTransaction")]
public partial class TTStockTransaction : BaseAuditableEntity
{
    [Key]
    public int StockTransactionID { get; set; }

    /// <summary>
    /// Ref TMStockType In, Out
    /// </summary>
    public int StockTypeID { get; set; }

    public int ItemID { get; set; }

    public int Qty { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TransactionDate { get; set; }

    [ForeignKey("ItemID")]
    [InverseProperty("TTStockTransactions")]
    public virtual TMItem Item { get; set; } = null!;

    [ForeignKey("StockTypeID")]
    [InverseProperty("TTStockTransactions")]
    public virtual TMStockType StockType { get; set; } = null!;
}
