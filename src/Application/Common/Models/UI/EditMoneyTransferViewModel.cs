using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
[JsonObject]
public class EditMoneyTransferViewModel
{
    public int MoneyTransferID { get; set; }

    [DisplayName("คลิก เพื่อแนบไฟล์รูปสลิปโอนเงิน")]
    public IFormFile? ImageFile { get; set; }

    [Required(ErrorMessage = "กรุณาระบุจำนวนเงินที่โอน")]
    [DisplayName("ระบุจำนวนเงินโอน")]
    public decimal AmountTransfer { get; set; }

    [Required(ErrorMessage = "กรุณาระบุวันที่โอน")]
    [DisplayName("ระบุวันที่โอน")]
    public string TransferDate { get; set; }

    [Required(ErrorMessage = "กรุณาระบุสาขาที่โอน")]
    [DisplayName("ระบุสาขา")]
    public int BranchID { get; set; }

    public bool IsActive { get; set; }
}
