using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;

[Serializable]
public class GetItemInBranchByBranchIDItemResponseDTO
{
	public int itemid { get; set; }

	public string itemcode { get; set; }

	public string itemname { get; set; }

	public string brandname { get; set; }

	public string brandshortname { get; set; }

	public decimal? price { get; set; }

	public double? discountpercent { get; set; }

	public int? qty { get; set; }
}
