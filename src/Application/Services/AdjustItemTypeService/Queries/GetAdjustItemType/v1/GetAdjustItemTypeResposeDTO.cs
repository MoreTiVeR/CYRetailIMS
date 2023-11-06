using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;

[Serializable]
public class GetAdjustItemTypeResposeDTO
{
    public int adjusttypeid { get; set; }

    public string adjusttypename { get; set; }

    public string description { get; set; }

    public string createdby { get; set; }

    public DateTime createddate { get; set; }

    public bool isactive { get; set; }
}
