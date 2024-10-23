
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTDraftItemTransfer")]
public partial class TTDraftItemTransfer : BaseAuditableEntity
{
    [Key]
    public int TransferHeaderID { get; set; }

    [StringLength(12)]
    [Unicode(false)]
    public string TransferRefNo { get; set; } = null!;

    /// <summary>
    /// Ref TMTransferType
    /// </summary>
    public int TransferTypeID { get; set; }

    /// <summary>
    /// WarehouseID, BranchID ต้นทาง
    /// </summary>
    public int SourceBranchID { get; set; }

    /// <summary>
    /// WarehouseID, BranchID ปลายทาง
    /// </summary>
    public int DestinationBranchID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    public int TransferStatus { get; set; }

    [InverseProperty("TransferHeader")]
    public virtual ICollection<TTDraftItemTransferDetail> TTDraftItemTransferDetails { get; set; } = new List<TTDraftItemTransferDetail>();
}
