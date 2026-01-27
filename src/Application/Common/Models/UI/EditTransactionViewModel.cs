using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
[JsonObject]
public class EditTransactionViewModel
{
    public int BranchID { get; set; }

    public int TransactionID { get; set; }

    [ReadOnly(true)]
    [Required(ErrorMessage = "*กรุณาระบุหมายเลขรายการขาย")]
    [DisplayName("ระบุหมายเลขรายการขาย")]
    public int ItemID { get; set; }

    [Required(ErrorMessage = "* กรุณาระวันที่ทำรายการ")]
    [Display(Name = "วันที่ทำรายการ")]
    public string TransactionDate { get; set; }

    [Display(Name = "ยอดเงินโอน")]
    public decimal AmountTransfer { get; set; }

    [Display(Name = "ยอดเงินฝาก")]
    public decimal AmountDeposit { get; set; }

    [Display(Name = "ค่าธรรมเนียม")]
    public decimal AmountFee { get; set; }

    [Display(Name = "เงินพนักงานพาร์ทไทม์")]
    public decimal AmountCash { get; set; }

    [Display(Name = "เงินรวม")]
    public decimal TotalAmount { get; set; }

    public string? Remark { get; set; }

    public bool CanUpdateTransaction { get; set; }

    public List<EditTransactionDetailViewModel> Detail { get; set; }


}
