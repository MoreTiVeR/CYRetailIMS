using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;

[Serializable]
public class GetSubItemTypeResponseDTO
{
    public int subitemtypeid { get; set; }
    public string subitemcode { get; set; }
    public string nameth { get; set; }
    public string nameen { get; set; }
    public string description { get; set; }
    public string createdby { get; set; }
    public DateTime createddate { get; set; }
    public string updatedby { get; set; }
    public DateTime updateddate { get; set; }
    public bool isactive { get; set; }
}
