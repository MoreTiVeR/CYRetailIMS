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

[JsonObject]
[Serializable]
public class CreateMoneyTransferViewModel
{
    //[Required(ErrorMessage = "กรุณาแนบรูปสลิปโอนเงิน")]
    [DisplayName("คลิก เพื่อแนบไฟล์รูปสลิปโอนเงิน")]
    public IFormFile[]? ImageFile { get; set; }

    [Required(ErrorMessage = "กรุณาระบุจำนวนเงินที่โอน")]
    [DisplayName("ระบุจำนวนเงินโอน")]
    public decimal AmountTransfer { get; set; }

    [Required(ErrorMessage = "กรุณาระบุวันที่โอน")]
    [DisplayName("ระบุวันที่โอน")]
    public string TransferDate { get; set; }

    [Required(ErrorMessage = "กรุณาระบุสาขาที่โอน")]
    [DisplayName("ระบุสาขา")]
    public int BranchID { get; set; }

    [DisplayName("ระบุรายละเอียด(ถ้ามี)")]
    public string? Description { get; set; }

    public string? SlipImagePath { get; set; }
}
