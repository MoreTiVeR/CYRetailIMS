
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTDraftItemTransferDetail")]
public partial class TTDraftItemTransferDetail : BaseAuditableEntity
{
    [Key]
    public int TransferDetailID { get; set; }

    public int TransferHeaderID { get; set; }

    public int ItemID { get; set; }

    public int Qty { get; set; }

    [ForeignKey("TransferHeaderID")]
    [InverseProperty("TTDraftItemTransferDetails")]
    public virtual TTDraftItemTransfer TransferHeader { get; set; } = null!;
}
