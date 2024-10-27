
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTItemTransferHeader")]
public partial class TTItemTransferHeader : BaseAuditableEntity
{
    [Key]
    public int TransferHeaderID { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string TransferRefNo { get; set; } = null!;

    public int TransferTypeID { get; set; }

    public int SourceBranchID { get; set; }

    public int DestinationBranchID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    public int TransferStatus { get; set; }

    [InverseProperty("TransferHeader")]
    public virtual ICollection<TTItemTransfer> TTItemTransfers { get; set; } = new List<TTItemTransfer>();
}
