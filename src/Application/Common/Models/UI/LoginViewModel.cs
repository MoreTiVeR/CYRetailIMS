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
    [DisplayName("ระบุชื่อผู้ใช้งาน")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุรหัสผ่าน")]
    [DisplayName("ระบุรหัสผ่าน")]
    public string Password { get; set; }
}
