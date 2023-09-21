using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class AccountRegisterViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุชื่อผู้ใช้งาน")]
    [DisplayName("ชื่อผู้ใช้งาน")]
    public string username { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุรหัสผ่าน")]
    [DisplayName("รหัสผ่าน")]
    public string password { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุชื่อจริง")]
    [DisplayName("ชื่อ")]
    public string firstname { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุนามกสุล")]
    [DisplayName("นามกสุล")]
    public string lastname { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุอีเมลที่ใช้งาน")]
    [DisplayName("อีเมล")]
    public string email { get; set; }

    //[Required(AllowEmptyStrings = false, ErrorMessage = "*กรุณาระบุแผนกพนักงาน")]
    //[DisplayName("แผนก")]
    //public string departmentid { get; set; }
    
    [Required(ErrorMessage = "*กรุณาระบุสิทธิ์เข้าใช้งาน")]
    [DisplayName("สิทธิ์การใช้งาน")]
    public int roleid { get; set; }

    [Required(ErrorMessage = "*กรุณาระบุสาขาที่เข้าใช้งาน")]
    [DisplayName("สาขาที่เข้าใช้งาน")]
    public int branchid { get; set; }
}
