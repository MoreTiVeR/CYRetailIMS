using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Common;

namespace CYRetailIMS.Domain.Entities;

[Table("TTEndOfDaySummary")]
public class TTEndOfDaySummary : BaseAuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("EndOfDayId")]
    public int EndOfDayId { get; set; }

    /// <summary>
    /// วันที่สรุปยอด
    /// </summary>
    [Required]
    [Column("SummaryDate", TypeName = "date")]
    public DateTime SummaryDate { get; set; }

    /// <summary>
    /// เงินสดรวมทั้งหมด
    /// </summary>
    [Required]
    [Column("TotalCash", TypeName = "decimal(18,2)")]
    public decimal TotalCash { get; set; }

    /// <summary>
    /// จำนวนเงินสดที่ฝากเข้าธนาคาร
    /// </summary>
    [Required]
    [Column("DepositedCash", TypeName = "decimal(18,2)")]
    public decimal DepositedCash { get; set; }

    /// <summary>
    /// เงินโอนรวมทั้งหมด
    /// </summary>
    [Required]
    [Column("TotalTransfer", TypeName = "decimal(18,2)")]
    public decimal TotalTransfer { get; set; }

    /// <summary>
    /// ยอดเงินที่ลูกค้าโอนจริง
    /// </summary>
    [Required]
    [Column("CustomerTransfer", TypeName = "decimal(18,2)")]
    public decimal CustomerTransfer { get; set; }

    /// <summary>
    /// ยอดรวมทั้งหมดก่อนหักค่าใช้จ่าย
    /// </summary>
    [Required]
    [Column("GrandTotal", TypeName = "decimal(18,2)")]
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// ค่าแรงคนแทน (ถ้ามี)
    /// </summary>
    [Column("SubstituteWage", TypeName = "decimal(18,2)")]
    public decimal? SubstituteWage { get; set; }

    /// <summary>
    /// ค่าธรรมเนียม
    /// </summary>
    [Column("Fee", TypeName = "decimal(18,2)")]
    public decimal? Fee { get; set; }

    /// <summary>
    /// ค่าใช้จ่ายอื่น ๆ
    /// </summary>
    [Column("OtherExpense", TypeName = "decimal(18,2)")]
    public decimal? OtherExpense { get; set; }

    /// <summary>
    /// หมายเหตุค่าใช้จ่ายอื่น ๆ
    /// </summary>
    [MaxLength(500)]
    [Column("OtherExpenseNote", TypeName = "NVARCHAR(500)")]
    public string? OtherExpenseNote { get; set; }

    /// <summary>
    /// ยอดรวมหลังหักค่าใช้จ่ายทั้งหมด
    /// </summary>
    [Required]
    [Column("FinalTotal", TypeName = "decimal(18,2)")]
    public decimal FinalTotal { get; set; }

    /// <summary>
    /// ผู้สร้างรายการ
    /// </summary>
    [Required]
    [MaxLength(10)]
    [Column("CreatedBy", TypeName = "VARCHAR(10)")]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// วันที่สร้าง
    /// </summary>
    [Required]
    [Column("CreatedDate", TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ผู้แก้ไขล่าสุด
    /// </summary>
    [MaxLength(10)]
    [Column("UpdatedBy", TypeName = "VARCHAR(10)")]
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// วันที่แก้ไขล่าสุด
    /// </summary>
    [Column("UpdatedDate", TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// สถานะการใช้งาน (1 = ใช้งาน, 0 = ปิด)
    /// </summary>
    [Required]
    [Column("IsActive")]
    public bool IsActive { get; set; }
}
