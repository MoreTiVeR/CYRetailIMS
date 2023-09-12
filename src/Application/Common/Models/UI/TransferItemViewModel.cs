using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class TransferItemViewModel
{
    [Required(ErrorMessage = "* กรุณาระบุคลังสินค้า/สาขา ต้นทาง")]
    [Display(Name = "ลังสินค้า/สาขา ต้นทาง")]
    public string source_branchid { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุคลังสินค้า/สาขา ปลายทาง")]
    [Display(Name = "ลังสินค้า/สาขา ปลายทาง")]
    public string destination_branchid { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุประเภทการโอนสินค้า")]
    [Display(Name = "ประเภทการโอนสินค้า")]
    public int transfertypeid { get; set; }

    //[Required(ErrorMessage = "* กรุณาระบุวันที่โอนสินค้า")]
    //[Display(Name = "วันที่โอนสินค้า")]
    public string transferdate { get; set; }

    //[Required(ErrorMessage = "* กรุณาระบุจำนวนสินค้า")]
    //[Display(Name = "จำนวนสินค้า")]
    public int qty { get; set; }

    //public string description { get; set; }

    [StringLength(50, ErrorMessage = "*ความยาวไม่เกิน 50 ตัวอักษร")]
    [DisplayName("ระบุหมายเหตุ(ถ้ามี)")]
    public string? description { get; set; }

    //[Required(ErrorMessage = "* กรุณาระบุสินค้าที่ต้องการโอน")]
    //[Display(Name = "สินค้าโอนสินค้า")]
    public string itembranchtransfer { get; set; }

}
