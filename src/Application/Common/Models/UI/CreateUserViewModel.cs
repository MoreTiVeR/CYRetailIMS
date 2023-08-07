using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;

public class CreateUserViewModel
{
	[Required(ErrorMessage = "*กรุณาระบุชื่อผู้ใช้งาน")]
	[RegularExpression("^[A-Za-z0-9].{4,20}$", ErrorMessage = "รูปแบบชื่อผู้ใช้งานไม่ถูกต้อง, ตัวอักษรภาษาอังกฤาและตัวเลข ความยาวไม่เกิน20ตัวอักษร")]
	[DisplayName("ระบุชื่อผู้ใช้งาน")]
	public string UserName { get; set; }

	[Required(ErrorMessage = "*กรุณาระบุรหัสผ่าน")]
	//[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "รุปแบบรหัสผ่านไม่ถูกต้อง, รหัสผ่านประกอบด้วยตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก ตัวเลข และ อัขระพิเศษ #?!@$%^&*- ความยาวขั้นต่ำ 8 ตัวอักษร")]
	[RegularExpression("^[A-Za-z0-9#?!@$%^&*-].{8,20}$", ErrorMessage = "รุปแบบรหัสผ่านไม่ถูกต้อง, รหัสผ่านประกอบด้วยตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก ตัวเลข และ อัขระพิเศษ #?!@$%^&*- ความยาวขั้นต่ำ 8 ตัวอักษร")]
	[DisplayName("ระบุรหัสผ่าน")]
	public string Password { get; set; }
}
