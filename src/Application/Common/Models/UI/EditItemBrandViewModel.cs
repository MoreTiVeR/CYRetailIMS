using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class EditItemBrandViewModel
{
    [ReadOnly(true)]
    [Required(ErrorMessage = "*กรุณาระบุรหัสแบรนด์สินค้า")]
    [DisplayName("ระบุชื่อแบรนด์")]
    public int brandid { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุชื่อแบรนด์")]
    [Display(Name = "ชื่อแบรนด์")]
    public string brandname { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุชื่อย่อแบรนด์")]
    [Display(Name = "ชื่อย่อแบรนด์")]
    public string brandshortname { get; set; }

    [Display(Name = "รายละเอียด/คำอธิบาย")]
    public string? description { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุสถานะใช้งาน")]
    [Display(Name = "สถานะ")]
    public string isactive { get; set; }
}