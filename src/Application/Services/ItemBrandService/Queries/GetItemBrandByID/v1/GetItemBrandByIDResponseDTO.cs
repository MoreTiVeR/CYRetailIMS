using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandByID.v1;

[Serializable]
public class GetItemBrandByIDResponseDTO
{
    public int brandid { get; set; }

    public string brandname { get; set; }

    public string brandshortname { get; set; }

    public string description { get; set; }

    public string createdby { get; set; }

    public DateTime createddate { get; set; }

    public bool isactive { get; set; }
}
