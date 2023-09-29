using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class EditUserViewModel
{
    public int UserID { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุชื่อผู้ใช้งาน")]
    [RegularExpression("^[A-Za-z0-9].{4,20}$", ErrorMessage = "รูปแบบชื่อผู้ใช้งานไม่ถูกต้อง, ตัวอักษรภาษาอังกฤษและตัวเลข ความยาวไม่เกิน20ตัวอักษร")]
    [DisplayName("ระบุชื่อผู้ใช้งาน")]
    public string UserName { get; set; }

    //[Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุรหัสผ่าน")]
    //[RegularExpression("^[A-Za-z0-9#?!@$%^&*-].{8,20}$", ErrorMessage = "รุปแบบรหัสผ่านไม่ถูกต้อง, รหัสผ่านประกอบด้วยตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก ตัวเลข และ อัขระพิเศษ #?!@$%^&*- ความยาวขั้นต่ำ 8 ตัวอักษร")]
    //[RegularExpression("^[A-Za-z0-9].{4,10}$", ErrorMessage = "รุปแบบรหัสผ่านไม่ถูกต้อง, ตัวอักษรภาษาอังกฤษและตัวเลข ความยาวไม่เกิน10ตัวอักษร")]
    [DisplayName("ระบุรหัสผ่าน")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุสิทธิ์การเข้าใช้งาน")]
    [DisplayName("สิทธิ์การเข้าใช้งาน")]
    public int RoleID { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุสาขาที่เข้าใช้งาน")]
    [DisplayName("สาขาที่เข้าใช้งาน")]
    public int BranchID { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุสถานะการใช้งาน")]
    public int IsActive { get; set; }

    public string UpdatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
}
