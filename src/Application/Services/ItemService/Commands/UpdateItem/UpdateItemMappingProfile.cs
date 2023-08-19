using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
public class UpdateItemMappingProfile : Profile
{
    public UpdateItemMappingProfile()
    {
        CreateMap<UpdateItemCommand, TMItem>()
            .ForMember(x => x.Name, f => f.MapFrom(ff => ff.name))
            .ForMember(x => x.ShortName, f => f.MapFrom(ff => ff.shortname))
            .ForMember(x => x.Description, f => f.MapFrom(ff => ff.description))
            .ForMember(x => x.Price, f => f.MapFrom(ff => ff.price))
            .ForMember(x => x.ItemImageUrl, f => f.MapFrom(ff => ff.itemimageurl))
            .ForMember(x => x.Qty, f => f.MapFrom(ff => ff.qty))
            .ForMember(x => x.DiscountPercent, f => f.MapFrom(ff => ff.discountpercent));
    }
}
