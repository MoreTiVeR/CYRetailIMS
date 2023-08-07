using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

[Table("TTItemTransfer")]
public partial class TTItemTransfer
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

    [StringLength(50)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreadedDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [Required]
    public bool? Status { get; set; }

    public int ApproveStatus { get; set; }

    [ForeignKey("TransferTypeID")]
    [InverseProperty("TTItemTransfers")]
    public virtual TMTransferType TransferType { get; set; } = null!;
}
