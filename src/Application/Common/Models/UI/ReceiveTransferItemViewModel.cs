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
public class ReceiveTransferItemViewModel
{
    [Required(ErrorMessage = "* กรุณาระบุหมายเลขการโอนให้ถูกต้อง")]
    public int TransferID { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุประเภทการโอนสินค้า")]
    [Display(Name = "ประเภทการโอนสินค้า")]
    public int TransferTypeID { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุคลังสินค้า/สาขา ต้นทาง")]
    [Display(Name = "ลังสินค้า/สาขา ต้นทาง")]
    public int SourceID { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุคลังสินค้า/สาขา ปลายทาง")]
    [Display(Name = "ลังสินค้า/สาขา ปลายทาง")]
    public int DestinationID { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุหมายเหตุ/รายละเอียดเพิ่มเติม")]
    [StringLength(50, ErrorMessage = "*ความยาวไม่เกิน 50 ตัวอักษร")]
    [DisplayName("ระบุหมายเหตุ")]
    public string Description { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุสินค้า")]
    [Display(Name = "สินค้าโอน")]
    public int ItemID { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุจำนวนสินค้าโอน")]
    [Display(Name = "จำนวนที่โอนสินค้า")]
    public int QTY { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุจำนวนสินค้ารับโอนจริง")]
    [Display(Name = "จำนวนสินค้าที่รับโอน")]
    public int ReceiveQTY { get; set; }

    [Display(Name = "จำนวนสินค้าที่คืน")]
    public int ReturnQTY { get; set; }

    [Required(ErrorMessage = "* กรุณาเลือกสถานะการโอนสินค้า")]
    [Display(Name = "สถานะการโอนสินค้า")]
    public int TransferStatusID { get; set; }

    public string ItemName { get; set; }

    public int TransferHeaderID { get; set; }
}
