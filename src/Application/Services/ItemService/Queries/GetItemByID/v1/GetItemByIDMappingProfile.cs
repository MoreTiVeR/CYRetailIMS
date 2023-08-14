using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
public class GetItemByIDMappingProfile : Profile
{
    public GetItemByIDMappingProfile()
    {
        CreateMap<TMItem, GetItemByIDResponseDTO>()
            .ForMember(m => m.itemid, f => f.MapFrom(x => x.ItemID))
            .ForMember(m => m.itemcode, f => f.MapFrom(x => x.ItemCode))
            .ForMember(m => m.itemtypeid, f => f.MapFrom(x => x.ItemTypeID))
            .ForMember(m => m.itemtypename, f => f.MapFrom(x => x.ItemType.ItemTypeName))
            .ForMember(m => m.unitofmeasureid, f => f.MapFrom(x => x.UnitOfMeasureID))
            .ForMember(m => m.unitofmeasurename, f => f.MapFrom(x => x.UnitOfMeasure.UnitOfMeasureName))
            .ForMember(m => m.brandid, f => f.MapFrom(x => x.Brand.BrandName))
            .ForMember(m => m.name, f => f.MapFrom(x => x.Name))
            .ForMember(m => m.shortname, f => f.MapFrom(x => x.ShortName))
            .ForMember(m => m.description, f => f.MapFrom(x => x.Description))
            .ForMember(m => m.barcode, f => f.MapFrom(x => x.BarCode))
            .ForMember(m => m.price, f => f.MapFrom(x => x.Price))
            .ForMember(m => m.itemimageurl, f => f.MapFrom(x => x.ItemImageUrl));
    }
}
