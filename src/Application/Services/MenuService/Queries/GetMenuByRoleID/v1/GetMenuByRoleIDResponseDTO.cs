using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;

[Serializable]
public class GetMenuByRoleIDResponseDTO
{
	public int menuid { get; set; }
	public int seq { get; set; }
	public string menuname_th { get; set; }
	public string menuname_en { get; set; }
	public object description { get; set; }
	public string cms_icon_name { get; set; }
	public string cms_link { get; set; }
	public string cms_title { get; set; }
	public bool isactive { get; set; }
	public List<SubMenuResponseDTO> submenulist { get; set; }

}
