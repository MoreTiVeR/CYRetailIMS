using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;
public class GetItemInBranchByBranchListMappingProfile : Profile
{

	public GetItemInBranchByBranchListMappingProfile()
	{
		CreateMap<TMItemInBranch, GetItemInBranchByBranchIDResponseDTO>()
			.ForMember(w => w.branchid, f => f.MapFrom(x => x.BranchID))
			.ForMember(w => w.branchname, f => f.MapFrom(x => x.Branch.BranchName))
			.AfterMap((s, d) => d.itemlist = new List<GetItemInBranchByBranchIDItemResponseDTO>
			{
				new GetItemInBranchByBranchIDItemResponseDTO
				{
					itemid = s.ItemID,
					itemcode = s.Item.ItemCode,
					itemname = s.Item.Name,
					brandname = s.Item.Brand.BrandName,
					brandshortname = s.Item.Brand.BrandShortName,
					price = s.Price,
					discountpercent = s.DiscountPercent,
					qty = s.Qty
				}
			});
	}
}
