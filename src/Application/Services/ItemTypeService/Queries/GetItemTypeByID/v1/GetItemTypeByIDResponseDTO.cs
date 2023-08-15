using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;

[Serializable]
public class GetItemTypeByIDResponseDTO
{
    public int itemtypeid { get; set; }

    public string itemtypename { get; set; }

    public string description { get; set; }
}
