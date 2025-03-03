using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CYRetailIMS.Domain.Common;

namespace CYRetailIMS.Domain.Entities;

[Table("TTItemTransferHistory")]
public partial class TTItemTransferHistory : BaseAuditableEntity
{
    [Key]
    public int TransferHistoryID { get; set; }

    public int TransferHeaderID { get; set; }

    public int BranchID { get; set; }

    public int ItemID { get; set; }

    [Required]
    [StringLength(12)]
    [Unicode(false)]
    public string ItemCode { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string ItemName { get; set; }

    public int? SubItemTypeID { get; set; }

    public int BrandID { get; set; }

    /// <summary>
    /// จำนวนในคลังใหญ่ ณ วันทำรายการ
    /// </summary>
    public int QtyInStock { get; set; }

    /// <summary>
    /// จำนวนในสาขาที่เหลือ ณ วันทำรายการ
    /// </summary>
    public int QtyInBranch { get; set; }

    /// <summary>
    /// จำนวนขั้นต่ำ
    /// </summary>
    public int NotifyMinQty { get; set; }

    /// <summary>
    /// จำนวนที่ต้องเติมตามระบบแนะนำ
    /// </summary>
    public int SuggestRefillQtyBySystem { get; set; }

    /// <summary>
    /// จำนวนที่เติมโดยผู้ทำรายการ
    /// </summary>
    public int RefillQty { get; set; }

}
