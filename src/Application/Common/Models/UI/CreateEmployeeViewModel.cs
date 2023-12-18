using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class CreateEmployeeViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุแผนก")]
    public int DepartmentID { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุชื่อจริง")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    public string FirstName { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุนามสกุล")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    public string LastName { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุอีเมล")]
    [RegularExpression("^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$", ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
    [MaxLength(50, ErrorMessage = "Maximum length 50")]
    public string Email { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุเบอร์มือถือ10หลัก")]
    [RegularExpression("^([0]{1})([1-9]{1})([0-9]{1})([0-9]{7})$", ErrorMessage = "เบอร์มือถือไม่ถูกต้อง")]
    [MaxLength(10, ErrorMessage = "Maximum length 10")]
    public string MobileNo { get; init; }

    public string? NickName { get; init; }

    [Required(ErrorMessage = "*กรุณาระบุสถานะการใช้งาน")]
    public int IsActive { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal Salary { get; set; }
}
