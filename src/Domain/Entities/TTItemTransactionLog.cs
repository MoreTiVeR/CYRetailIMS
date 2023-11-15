using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTItemTransactionLogs")]
public partial class TTItemTransactionLog : BaseAuditableEntity
{
    [Key]
    public int ID { get; set; }

    public int ItemID { get; set; }

    public int BranchID { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal OldPrice { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal NewPrice { get; set; }
}
