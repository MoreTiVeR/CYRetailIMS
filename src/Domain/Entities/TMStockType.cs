using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMStockType")]
public partial class TMStockType : BaseAuditableEntity
{
    [Key]
    public int StockTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string StockTypeName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("StockType")]
    public virtual ICollection<TTStockTransaction> TTStockTransactions { get; set; } = new List<TTStockTransaction>();
}
