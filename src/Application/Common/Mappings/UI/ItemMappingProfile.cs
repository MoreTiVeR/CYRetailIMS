using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.ItemService.Queries.GetItemList.v1;

namespace CYRetailIMS.Application.Common.Mappings.UI;
public class ItemMappingProfile : Profile
{
    public ItemMappingProfile() 
    {
        CreateMap<GetItemListResponseDTO, EditItemViewModel>()
            .ForMember(x => x.ItemCode, f => f.MapFrom(ff => ff.itemid))
            .ForMember(x => x.ItemTypeID, f => f.MapFrom(ff => ff.itemtypeid))
            .ForMember(x => x.BrandID, f => f.MapFrom(ff => ff.brandid))
            .ForMember(x => x.UnitOfMeasureID, f => f.MapFrom(ff => ff.unitofmeasureid))
            .ForMember(x => x.Description, f => f.MapFrom(ff => ff.description))
            .ForMember(x => x.ItemCode, f => f.MapFrom(ff => ff.itemcode))
            .ForMember(x => x.ItemImageUrl, f => f.MapFrom(ff => ff.itemimageurl))
            .ForMember(x => x.Price, f => f.MapFrom(ff => ff.price))
            .ForMember(x => x.DiscountPercent, f => f.MapFrom(ff => ff.discountpercent))
            .ForMember(x => x.ShortName, f => f.MapFrom(ff => ff.shortname))
            .ForMember(x => x.Qty, f => f.MapFrom(ff => ff.qty))
            .ForMember(x => x.Cost, f => f.MapFrom(ff => ff.cost))
            .ForMember(x => x.IsActive, f => f.MapFrom(ff => ff.isactive));
    }
}
