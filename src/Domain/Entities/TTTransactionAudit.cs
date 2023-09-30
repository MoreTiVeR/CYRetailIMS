using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTTransactionAudit")]
public partial class TTTransactionAudit : BaseAuditableEntity
{
    [Key]
    public int AuditID { get; set; }

    public int TransactionID { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal TotalAuditAmount { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Description { get; set; }

    [ForeignKey("TransactionID")]
    [InverseProperty("TTTransactionAudits")]
    public virtual TTTransaction Transaction { get; set; } = null!;
}
