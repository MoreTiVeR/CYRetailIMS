using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTMoneyTransferSlipsDetail")]
public partial class TTMoneyTransferSlipsDetail : BaseAuditableEntity
{
    [Key]
    public int MoneyTransferSlipDetailID { get; set; }

    public int MoneyTransferSlipID { get; set; }

    [StringLength(80)]
    [Unicode(false)]
    public string SlipImagePath { get; set; } = null!;


    [ForeignKey("MoneyTransferSlipID")]
    [InverseProperty("TTMoneyTransferSlipsDetails")]
    public virtual TTMoneyTransferSlip MoneyTransferSlip { get; set; } = null!;
}
