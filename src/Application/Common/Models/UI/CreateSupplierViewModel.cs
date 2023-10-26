using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CreateSupplierViewModel
{

    [Required(ErrorMessage = "กรุณาระบุชื่อซัฟพลายเออร์ภาษาไทย", AllowEmptyStrings = false)]
    public string suppliername_th { get; set; }

    //[Required(ErrorMessage = "กรุณาระบุชื่อซัฟพลายเออร์ภาษาอังกฤษ", AllowEmptyStrings = false)]
    public string? suppliername_en { get; set; }

    //[Required(ErrorMessage = "กรุณาระบุประเภทซัฟพลายเออร์")]
    public int? suppliertypeid { get; set; }

    [DisplayName("กรุณาระบุที่อยู่ซัฟพลายเออร์")]
    [Required(ErrorMessage = "กรุณาระบุที่อยู่", AllowEmptyStrings = false)]
    public string description { get; set; }

    #region Contact

    //[Required(ErrorMessage = "กรุณาระบุประเภทารติดต่อซัฟพลายเออร์")]
    public int? suppliercontacttypeid { get; init; }

    //[Required(ErrorMessage = "กรุณาระบุบัญชี/Line/Facebook ซัฟพลายเออร์", AllowEmptyStrings = false)]
    public string? contactaccountname { get; init; }

    [Required(ErrorMessage = "กรุณาระบุชื่อผู้ติดต่อ")]
    public string? contactperson { get; init; }

    //[MinLength(10, ErrorMessage = "กรุณาระบุเบอร์ติดต่อซัฟพลายเออร์")]
    //[MaxLength(13, ErrorMessage ="กรุณาระบุเบอร์ติดต่อซัฟพลายเออร์")]
    //[Required(ErrorMessage = "กรุณาระบุเบอร์ติดต่อ")]
    [DisplayName("กรุณาระบุเบอร์ติดต่อ(ถ้าไม่มีปล่อยว่าง)")]
    public string? mobileno { get; init; }

    #endregion
}
