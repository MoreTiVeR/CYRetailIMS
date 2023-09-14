using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CYRetailIMS.Application.Common.Models.UI;

[Serializable]
public class CreateItemBrandViewModel
{
	[Required(ErrorMessage = "* กรุณาระบุชื่อแบรนด์")]
	[Display(Name = "ชื่อแบรนด์")]
	public string brandname { get; set; }

	[Required(ErrorMessage = "* กรุณาระบุชื่อย่อแบรนด์")]
	[Display(Name = "ชื่อย่อแบรนด์")]
	public string brandshortname { get; set; }

	[Display(Name = "รายละเอียด/คำอธิบาย")]
	public string description { get; set; }

}
