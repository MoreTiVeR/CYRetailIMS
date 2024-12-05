using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TTMoneyTransferSlip : BaseAuditableEntity
{
    [Key]
    public int MoneyTransferSlipID { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal TotalAmountTransfer { get; set; }

    [InverseProperty("MoneyTransferSlip")]
    public virtual ICollection<TTMoneyTransferSlipsDetail> TTMoneyTransferSlipsDetails { get; set; } = new List<TTMoneyTransferSlipsDetail>();

    [InverseProperty("MoneyTransferSlip")]
    public virtual ICollection<TTMoneyTransfer> TTMoneyTransfers { get; set; } = new List<TTMoneyTransfer>();
}
