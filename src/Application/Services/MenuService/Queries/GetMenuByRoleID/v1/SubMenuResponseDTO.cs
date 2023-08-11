using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
public class SubMenuResponseDTO
{
    [JsonPropertyName("submenuid")]
    public int SubMenuID { get; set; }

    //[JsonPropertyName("menuid")]
    //public int MenuID { get; set; }

    [JsonPropertyName("seq")]
    public int Seq { get; set; }

    [JsonPropertyName("menuname_en")]
    public string MenuName_EN { get; set; } = null!;

    [JsonPropertyName("menuname_th")]
    public string MenuName_TH { get; set; } = null!;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("cms_controllername")]
    public string? CMS_ControllerName { get; set; }

    [JsonPropertyName("cms_actionname")]
    public string? CMS_ActionName { get; set; }

    [JsonPropertyName("cms_i_class")]
    public string? CMS_I_Class { get; set; }

    [JsonPropertyName("cms_span_class")]
    public string? CMS_Span_Class { get; set; }

    [JsonPropertyName("cms_link")]
    public string? CMS_Link { get; set; }

    [JsonPropertyName("isactive")]
    public bool IsActive { get; set; }
}
