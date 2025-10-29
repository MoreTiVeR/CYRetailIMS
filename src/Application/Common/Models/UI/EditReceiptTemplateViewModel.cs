using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Models.UI;
public class EditReceiptTemplateViewModel
{
    public int receivetempid { get; set; }
    public int branchid { get; set; }
    //public string branchcode { get; set; }
    //public string branchname { get; set; }

    [StringLength(50, ErrorMessage = "*ความยาวไม่เกิน 50 ตัวอักษร")]
    [DisplayName("*ระบุชื่อบริษัท")]
    public string shopheadernametext { get; set; }

    [StringLength(200, ErrorMessage = "*ความยาวไม่เกิน 200 ตัวอักษร")]
    [DisplayName("*ระบุที่อยู่บริษัท")]
    public string shopheaderaddresstext { get; set; }
    //public string? additionalheadertext { get; set; }

    [StringLength(50, ErrorMessage = "*ความยาวไม่เกิน 50 ตัวอักษร")]
    [DisplayName("*ระบุข้อความท้ายกระดาษ")]
    public string? shopfootertext { get; set; }
    public string? additionalfootertext { get; set; }

    [StringLength(20, ErrorMessage = "*ความยาวไม่เกิน 20 ตัวอักษร")]
    [DisplayName("*ระบุเบอร์โทรศัพท์")]
    public string telephoneno { get; set; }

    [StringLength(20, ErrorMessage = "*ความยาวไม่เกิน 20 ตัวอักษร")]
    [DisplayName("*ระบุเชื่อเครื่องปริ้น")]
    public string printername { get; set; }
    public string? updatedby { get; set; }
    public string isactive { get; set; }
}
