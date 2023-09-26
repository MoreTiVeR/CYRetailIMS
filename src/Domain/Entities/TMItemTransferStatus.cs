using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMItemTransferStatus")]
public partial class TMItemTransferStatus : BaseAuditableEntity
{
    [Key]
    public int TransferStatusID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TransferStatusName_TH { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string TransferStatusName_EN { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }
}
