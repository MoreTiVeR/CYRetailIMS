using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

public class EditSupplierViewModel
{
    [Required(ErrorMessage = "กรุณาระบุไอดีซัฟพลายเออร์")]
    public int supplierid { get; set; }

    [Required(ErrorMessage = "กรุณาระบุชื่อซัฟพลายเออร์ภาษาไทย", AllowEmptyStrings = false)]
    public string suppliername_th { get; set; }

    [Required(ErrorMessage = "กรุณาระบุชื่อซัฟพลายเออร์ภาษาอังกฤษ", AllowEmptyStrings = false)]
    public string suppliername_en { get; set; }

    [Required(ErrorMessage = "กรุณาระบุประเภทซัฟพลายเออร์")]
    public int suppliertypeid { get; set; }

    public string? suppliertypename { get; set; }

    public string? description { get; set; }

    #region Contact

    [Required(ErrorMessage = "กรุณาระบุประเภทารติดต่อซัฟพลายเออร์")]
    public int suppliercontacttypeid { get; init; }

    [Required(ErrorMessage = "กรุณาระบุบัญชี/Line/Facebook ซัฟพลายเออร์", AllowEmptyStrings = false)]
    public string contactaccountname { get; init; }

    //[Required(ErrorMessage = "กรุณาระบุชื่อผู้ติดต่อ", AllowEmptyStrings = false)]
    public string? contactperson { get; init; }

    public string? mobileno { get; init; }

    #endregion
}
