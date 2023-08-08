using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
[JsonObject]
public class LoginViewModel
{
    [Required(ErrorMessage = "*กรุณาระบุชื่อผู้ใช้งาน")]
    //[RegularExpression("^[A-Za-z0-9]*$", ErrorMessage ="รุปแบบชื่อผู้ใช้งานไม่ถูกต้อง")]
    [DisplayName("ระบุชื่อผู้ใช้งาน")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุรหัสผ่าน")]
	//[RegularExpression("^[A-Za-z0-9#?!@$%^&*-].{8,20}$", ErrorMessage = "รุปแบบรหัสผ่านไม่ถูกต้อง")]
	//[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$\r\n", ErrorMessage = "รุปแบบรหัสผ่านไม่ถูกต้อง, รหัสผ่านประกอบด้วยตัวพิมพ์ใหญ่ ตัวพิมพ์เล็ก ตัวเลข และ อัขระพิเศษ #?!@$%^&*-")]
	[DisplayName("ระบุรหัสผ่าน")]
    public string Password { get; set; }
}
