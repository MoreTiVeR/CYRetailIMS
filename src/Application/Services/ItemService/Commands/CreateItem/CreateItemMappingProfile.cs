using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
public class CreateItemMappingProfile : Profile
{
    public CreateItemMappingProfile()
    {
        //CreateMap<CreateItemCommand, TMItem>()
        //    .ForMember(m => m.ItemCode, f => f.MapFrom(x => x.itemcode))
        //    .ForMember(m => m.ItemTypeID, f => f.MapFrom(x => x.itemtypeid))
        //    .ForMember(m => m.UnitOfMeasureID, f => f.MapFrom(x => x.unitofmeasureid))
        //    .ForMember(m => m.BrandID, f => f.MapFrom(x => x.brandid))
        //    .ForMember(m => m.Name, f => f.MapFrom(x => x.name))
        //    .ForMember(m => m.ShortName, f => f.MapFrom(x => x.shortname))
        //    .ForMember(m => m.Description, f => f.MapFrom(x => x.description))
        //    .ForMember(m => m.BarCode, f => f.MapFrom(x => x.barcode))
        //    .ForMember(m => m.Price, f => f.MapFrom(x => x.price))
        //    .ForMember(m => m.Qty, f => f.MapFrom(x => x.qty))
        //    .ForMember(m => m.NotifyMinQty, f => f.MapFrom(x => x.notifyminqty))
        //    .ForMember(m => m.DiscountPercent, f => f.MapFrom(x => x.discountpercent))
        //    .ForMember(m => m.ItemImageUrl, f => f.MapFrom(x => !string.IsNullOrEmpty(x.itemimageurl) ? x.itemimageurl : "../assets/img/product/noimage.png"))
        //    .ForMember(m => m.CreatedBy, f => f.MapFrom(x => x.createdby))
        //    .ForMember(m => m.IsActive, f => f.MapFrom(x => x.isactive));

		CreateMap<CreateItemDetailCommand, TMItem>()
	        .ForMember(m => m.ItemCode, f => f.MapFrom(x => x.itemcode))
	        .ForMember(m => m.ItemTypeID, f => f.MapFrom(x => x.itemtypeid))
	        .ForMember(m => m.UnitOfMeasureID, f => f.MapFrom(x => x.unitofmeasureid))
	        .ForMember(m => m.BrandID, f => f.MapFrom(x => x.brandid))
	        .ForMember(m => m.Name, f => f.MapFrom(x => x.name))
	        .ForMember(m => m.ShortName, f => f.MapFrom(x => x.shortname))
	        .ForMember(m => m.Description, f => f.MapFrom(x => x.description))
	        .ForMember(m => m.BarCode, f => f.MapFrom(x => x.barcode))
	        .ForMember(m => m.Price, f => f.MapFrom(x => x.price))
	        .ForMember(m => m.Qty, f => f.MapFrom(x => x.qty))
	        .ForMember(m => m.NotifyMinQty, f => f.MapFrom(x => x.notifyminqty))
	        .ForMember(m => m.DiscountPercent, f => f.MapFrom(x => x.discountpercent))
	        .ForMember(m => m.ItemImageUrl, f => f.MapFrom(x => !string.IsNullOrEmpty(x.itemimageurl) ? x.itemimageurl : "../assets/img/product/noimage.png"))
	        .ForMember(m => m.CreatedBy, f => f.MapFrom(x => x.createdby))
	        .ForMember(m => m.IsActive, f => f.MapFrom(x => x.isactive));
	}
}
