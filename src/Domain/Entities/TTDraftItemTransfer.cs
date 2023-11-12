using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTDraftItemTransfer")]
public partial class TTDraftItemTransfer : BaseAuditableEntity
{
    [Key]
    public int TransferID { get; set; }

    /// <summary>
    /// Ref TMTransferType
    /// </summary>
    public int TransferTypeID { get; set; }

    /// <summary>
    /// WarehouseID, BranchID ต้นทาง
    /// </summary>
    public int SourceID { get; set; }

    /// <summary>
    /// WarehouseID, BranchID ปลายทาง
    /// </summary>
    public int DestinationID { get; set; }

    public int ItemID { get; set; }

    public int Qty { get; set; }

    public int? ReceiveQTY { get; set; }

    public int? ReturnQTY { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Required]
    public bool? IsActive { get; set; }

    public int TransferStatus { get; set; }
}
