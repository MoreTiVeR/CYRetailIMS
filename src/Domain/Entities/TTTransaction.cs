using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TTTransaction : BaseAuditableEntity
{
    [Key]
    public int TransactionID { get; set; }

    public int TransactionTypeID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TransactionDate { get; set; }

    public int BranchID { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal AmountTransfer { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal AmountDeposit { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal AmountCash { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal Fee { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Vat { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Discount { get; set; }

    public double? DiscountPercent { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal TotalAmount { get; set; }

    public bool IsExcludeVAT { get; set; }

    //[StringLength(10)]
    //[Unicode(false)]
    //public string CreatedBy { get; set; } = null!;

    //[Column(TypeName = "datetime")]
    //public DateTime CreadedDate { get; set; }

    //[StringLength(10)]
    //[Unicode(false)]
    //public string? UpdatedBy { get; set; }

    //[Column(TypeName = "datetime")]
    //public DateTime? UpdatedDate { get; set; }

    //[Required]
    //public bool? IsActive { get; set; }

    [InverseProperty("Transaction")]
    public virtual ICollection<TTTransactonDetail> TTTransactonDetails { get; set; } = new List<TTTransactonDetail>();

    [ForeignKey("TransactionTypeID")]
    [InverseProperty("TTTransactions")]
    public virtual TMTransactionType TransactionType { get; set; } = null!;
}
