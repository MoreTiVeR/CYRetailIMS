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
    [JsonPropertyName("menuid")]
    public int MenuID { get; set; }

    [JsonPropertyName("seq")]
    public int Seq { get; set; }

    [JsonPropertyName("menuname_th")]
    public string MenuName_TH { get; set; }

    [JsonPropertyName("menuname_en")]
    public string MenuName_EN { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("cms_icon_name")]
    public string CMS_DataIconName { get; set; }

    [JsonPropertyName("cms_link")]
    public string CMS_Link { get; set; }

    [JsonPropertyName("cms_title")]
    public string CMS_Title { get; set; }

    [JsonPropertyName("isactive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("submenulist")]
    public List<SubMenuResponseDTO> SubMenuList { get; set; }

}
