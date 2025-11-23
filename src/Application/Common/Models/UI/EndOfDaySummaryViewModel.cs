using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class EndOfDaySummaryViewModel
{
    public int? EndOfDayId { get; set; } // null = Create, มีค่า = Edit

    [Required(ErrorMessage = "กรุณาเลือกวันที่สรุปยอด")]
    [DataType(DataType.Text)]
    [Display(Name = "วันที่สรุปยอด")]
    public string SummaryDate { get; set; }

    [Required(ErrorMessage = "กรุณากรอกเงินสดรวม")]
    [Display(Name = "เงินสดรวม")]
    public decimal TotalCash { get; set; }

    [Required(ErrorMessage = "กรุณากรอกเงินสดฝาก")]
    [Display(Name = "เงินสดฝาก")]
    public decimal DepositedCash { get; set; }

    [Required(ErrorMessage = "กรุณากรอกเงินโอนรวม")]
    [Display(Name = "เงินโอนรวม")]
    public decimal TotalTransfer { get; set; }

    [Required(ErrorMessage = "กรุณากรอกเงินลูกค้าโอน")]
    [Display(Name = "เงินลูกค้าโอน")]
    public decimal CustomerTransfer { get; set; }

    [Required(ErrorMessage = "กรุณากรอกยอดรวมทั้งหมด")]
    [Display(Name = "รวมทั้งหมด")]
    public decimal GrandTotal { get; set; }

    [Display(Name = "ค่าแรงคนแทน")]
    public decimal? SubstituteWage { get; set; }

    [Display(Name = "ค่าธรรมเนียม")]
    public decimal? Fee { get; set; }

    [Display(Name = "ค่าอื่น ๆ")]
    public decimal? OtherExpense { get; set; }

    [MaxLength(500, ErrorMessage = "หมายเหตุต้องไม่เกิน 500 ตัวอักษร")]
    [Display(Name = "หมายเหตุค่าอื่น ๆ")]
    public string? OtherExpenseNote { get; set; }

    [Required(ErrorMessage = "กรุณากรอกยอดรวมสุดท้าย")]
    [Display(Name = "รวมสุทธิ")]
    public decimal FinalTotal { get; set; }

    [Display(Name = "สถานะการใช้งาน")]
    public bool IsActive { get; set; } = true;

    // ให้ Controller ใส่ค่าเองจาก User Login
    public string? CurrentUserName { get; set; }
}
