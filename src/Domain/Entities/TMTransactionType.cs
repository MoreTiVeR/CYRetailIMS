using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMTransactionType")]
public partial class TMTransactionType : BaseAuditableEntity
{
    [Key]
    public int TransactionTypeID { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string TransactionTypeCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string TransactionTypeName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    //[StringLength(10)]
    //[Unicode(false)]
    //public string CreatedBy { get; set; } = null!;

    //[Column(TypeName = "datetime")]
    //public DateTime CreatedDate { get; set; }

    //[StringLength(10)]
    //[Unicode(false)]
    //public string? UpdatedBy { get; set; }

    //[Column(TypeName = "datetime")]
    //public DateTime? UpdatedDate { get; set; }

    //[Required]
    //public bool? IsActive { get; set; }

    [InverseProperty("TransactionType")]
    public virtual ICollection<TTTransaction> TTTransactions { get; set; } = new List<TTTransaction>();
}
