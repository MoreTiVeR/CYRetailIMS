using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TTMoneyTransfer : BaseAuditableEntity
{
    [Key]
    public int MoneyTransferID { get; set; }

    public int BranchID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TransferDate { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal AmountTransfer { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [ForeignKey("BranchID")]
    [InverseProperty("TTMoneyTransfers")]
    public virtual TMBranch Branch { get; set; } = null!;
}
