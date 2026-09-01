using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CYRetailIMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Domain.Entities;

public partial class TTCountStock : BaseAuditableEntity
{
    [Key]
    public int CountStockID { get; set; }

    public int BranchID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CountDate { get; set; }

    public int TotalCount { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Remark { get; set; }

    /// <summary>
    /// สถานะการนับสต๊อก: 0=Draft, 1=Submitted, 2=Approved
    /// </summary>
    public int CountStockStatusID { get; set; } = 0;

    /// <summary>
    /// บทบาทของผู้นับ: "PC" = พนักงานขาย, "HeadPC" = หัวหน้า PC
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string? CounterRole { get; set; }

    /// <summary>
    /// วันที่อนุมัติ
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? ApprovedDate { get; set; }

    /// <summary>
    /// ผู้อนุมัติ
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string? ApprovedBy { get; set; }

    [InverseProperty("CountStock")]
    public virtual ICollection<TTCountStockDetail> TTCountStockDetails { get; set; } = new List<TTCountStockDetail>();
}
