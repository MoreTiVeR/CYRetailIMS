using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TMTransferType")]
public partial class TMTransferType : BaseAuditableEntity
{
    [Key]
    public int TransferTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TransferTypeName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("TransferType")]
    public virtual ICollection<TTItemTransfer> TTItemTransfers { get; set; } = new List<TTItemTransfer>();
}
