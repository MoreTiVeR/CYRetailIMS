using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
public class GetItemInBranchByBranchIDMappingProfile : Profile
{
	public GetItemInBranchByBranchIDMappingProfile()
	{
		//CreateMap<TMItemInBranch, GetItemInBranchByBranchIDItemResponseDTO>();

		//CreateMap<TMItemBrand, GetItemInBranchByBranchIDItemResponseDTO>()
		//	.ForMember(w => w.itemid, f => f.MapFrom(x => x.br))
		//.ForMember(w => w.brandname, f => f.MapFrom(x => x.BrandName))
		//.ForMember(w => w.brandshortname, f => f.MapFrom(x => x.BrandShortName));

		//CreateMap<TMItemInBranch, GetItemInBranchByBranchIDResponseDTO>()
		//	.ForMember(w => w.branchid, f => f.MapFrom(x => x.BranchID))
		//	.ForMember(w => w.branchname, f => f.MapFrom(x => x.Branch.BranchName))
		//	.ForMember(w => w.itemlist, f => f.MapFrom(x => new GetItemInBranchByBranchIDItemResponseDTO
		//	{
		//		itemid = x.ItemID,
		//		itemcode = x.Item.ItemCode,
		//		itemname = x.Item.Name,
		//		brandname = x.Item.Brand.BrandName,
		//		brandshortname = x.Item.Brand.BrandShortName,
		//		price = x.Price,
		//		qty = x.Qty,
		//		discountpercent = x.DiscountPercent
		//	}));
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

		//.ForMember(w => w.itemid, f => f.MapFrom(x => x.ItemID))
		//.ForMember(w => w.itemcode, f => f.MapFrom(x => x.Item.ItemCode))
		//.ForMember(w => w.itemname, f => f.MapFrom(x => x.Item.Name))
		//.ForMember(w => w.brandname, f => f.MapFrom(x => x.Item.Brand.BrandName))
		//.ForMember(w => w.brandshortname, f => f.MapFrom(x => x.Item.Brand.BrandShortName))
		//.ForMember(w => w.qty, f => f.MapFrom(x => x.Qty))
		//.ForMember(w => w.price, f => f.MapFrom(x => x.Price))
		//.ForMember(w => w.discountpercent, f => f.MapFrom(x => x.DiscountPercent));


	}
}
