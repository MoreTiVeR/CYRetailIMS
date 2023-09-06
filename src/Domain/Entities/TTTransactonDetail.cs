using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTTransactonDetail")]
public partial class TTTransactonDetail : BaseEntity
{
    [Key]
    public int TransactionDetailID { get; set; }

    public int TransactionID { get; set; }

    public int ItemID { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    public int Qty { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Discount { get; set; }

    public double? DiscountPercent { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Amount { get; set; }

    [Required]
    public bool? IsActive { get; set; }

    [ForeignKey("TransactionID")]
    [InverseProperty("TTTransactonDetails")]
    public virtual TTTransaction Transaction { get; set; } = null!;
}
