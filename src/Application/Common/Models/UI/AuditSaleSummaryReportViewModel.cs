using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
[JsonObject]
public class AuditSaleSummaryReportViewModel
{
    #region Transaction
    [Required(ErrorMessage = "* กรุณาระบุเลขประวัติการทำรายการ")]
    public int TransactionID { get; set; }

    [Required(ErrorMessage = "* กรุณาระวันที่ทำรายการ")]
    [Display(Name = "วันที่ทำรายการ")]
    public string TransactionDate { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุยอดเงินรวม")]
    [Display(Name = "เงินรวม")]
    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุยอดเงินโอน")]
    [Display(Name = "เงินโอน")]
    public decimal AmountTransfer { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุยอดเงินฝากธนาคาร")]
    [Display(Name = "เงินฝาก")]
    public decimal AmountDeposit { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุยอดเงินสดคงเหลือ")]
    [Display(Name = "เงินสด")]
    public decimal AmountCash { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุค่าธรรมเนียมเงินฝาก")]
    [Display(Name = "ค่าธรรมเนียม")]
    public decimal DepositFee { get; set; }

    public string CreatedBy { get; set; }
    public string CreatedbyStaff { get; set; }
    #endregion

    #region Branch
    [Required(ErrorMessage = "* กรุณาระบุสาขา")]
    [Display(Name = "สาขา")]
    public int BranchID { get; set; }

    public string BranchName { get; set; }
    #endregion


    #region Audit
    public int? AuditID { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุยอดเงินบัญชีตรวจสอบ")]
    [Display(Name = "ยอดเงินบัญชี")]
    public decimal? TotalAuditAmount { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุหมายเหตุ/รายละเอียดเพิ่มเติม")]
    [StringLength(200, ErrorMessage = "*ความยาวไม่เกิน 200 ตัวอักษร")]
    [DisplayName("ระบุหมายเหตุ(ถ้ามี)")]
    public string AuditDescription { get; set; }
    #endregion
}
