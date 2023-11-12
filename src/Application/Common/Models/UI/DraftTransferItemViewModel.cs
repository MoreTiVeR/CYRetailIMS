using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

public class DraftTransferItemViewModel
{
    [StringLength(50, ErrorMessage = "*ความยาวไม่เกิน 50 ตัวอักษร")]
    [DisplayName("ระบุหมายเหตุ(ถ้ามี)")]
    public string description { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุคลังสินค้า/สาขา ปลายทาง")]
    [Display(Name = "ลังสินค้า/สาขา ปลายทาง")]
    public string destination_branchid { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุคลังสินค้า/สาขา ต้นทาง")]
    [Display(Name = "ลังสินค้า/สาขา ต้นทาง")]
    public string source_branchid { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุประเภทการโอนสินค้า")]
    [Display(Name = "ประเภทการโอนสินค้า")]
    public int transfertypeid { get; set; }

}