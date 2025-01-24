using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeByItemIDList.v1;
public class GetSubItemTypeByItemIDListResponseDTO
{
    public int itemid { get; set; }
    public string itemname { get; set; }
    public int? subitemtypeid { get; set; }
    public string? subitemcode { get; set; }
    public string? nameth { get; set; }
    public string? nameen { get; set; }
}
