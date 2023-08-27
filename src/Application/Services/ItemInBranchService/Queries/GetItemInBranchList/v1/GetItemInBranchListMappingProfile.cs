using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchList.v1;
public class GetItemInBranchListMappingProfile : Profile
{
	public GetItemInBranchListMappingProfile()
	{
		//CreateMap<TMItemInBranch, GetItemInBranchListResponseDTO>()
		//			.ForMember(w => w.branchid, f => f.MapFrom(x => x.BranchID))
		//			.ForMember(w => w.branchname, f => f.MapFrom(x => x.Branch.BranchName))
		//			.ForMember(w => w.itemlist, f => f.MapFrom(x => new GetItemInBranchByBranchIDItemResponseDTO
		//			{
		//				itemid = x.ItemID,
		//				itemcode = x.Item.ItemCode,
		//				itemname = x.Item.Name,
		//				brandname = x.Item.Brand.BrandName,
		//				brandshortname = x.Item.Brand.BrandShortName,
		//				price = x.Price,
		//				qty = x.Qty,
		//				discountpercent = x.DiscountPercent
		//			}));
		CreateMap<TMItemInBranch, GetItemInBranchListResponseDTO>()
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
