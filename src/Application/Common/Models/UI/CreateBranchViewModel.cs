using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class CreateBranchViewModel
{
	[Required(ErrorMessage = "* กรุณาระบุรหัสสาขาาขา")]
	[Display(Name = "รหัสสาขา")]
	public string branchcode { get; set; }

	[Required(ErrorMessage = "* กรุณาระบุเชื่อสาขา")]
	[Display(Name = "ชื่อสาขา")]
	public string branchname { get; set; }
}
