using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class EditBranchViewModel
{
    [Required(ErrorMessage = "* กรุณาระบุไอดีสาขา")]
    public int branchid { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุรหัสสาขาาขา")]
    [Display(Name = "รหัสสาขา")]
    public string branchcode { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุเชื่อสาขา")]
    [Display(Name = "ชื่อสาขา")]
    public string branchname { get; set; }

    [Required(ErrorMessage = "* กรุณาระบุที่อยู่สาขา")]
    [Display(Name = "ที่อยู่สาขา")]
    public string address { get; set; }

    #region Phase 2
    //[Required(ErrorMessage = "* กรุณาระบุตำบล")]
    //[Display(Name = "ตำบล")]
    //public int subdistrictid { get; set; }

    //[Required(ErrorMessage = "* กรุณาระบุอำเภอ")]
    //[Display(Name = "อำเภอ")]
    //public int districtid { get; set; }

    //[Required(ErrorMessage = "* กรุณาระบุจังหวัด")]
    //[Display(Name = "จังหวัด")]
    //public int provinceid { get; set; }

    //[Required(ErrorMessage = "* กรุณาระบุรหัสไปรษณีย์")]
    //[Display(Name = "รหัสไปรษณีย์")]
    //public int zipcode { get; set; }

    //[Required(ErrorMessage = "* กรุณาระบุภาค")]
    //[Display(Name = "ภาค")]
    //public int geoid { get; set; }
    #endregion
}
