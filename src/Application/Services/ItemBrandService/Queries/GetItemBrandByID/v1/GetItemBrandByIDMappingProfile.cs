using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandByID.v1;
public class GetItemBrandByIDMappingProfile : Profile
{
    public GetItemBrandByIDMappingProfile()
    {
        CreateMap<TMItemBrand, GetItemBrandByIDResponseDTO>()
            .ForMember(m => m.brandid, f => f.MapFrom(w => w.BrandID))
            .ForMember(m => m.brandname, f => f.MapFrom(w => w.BrandName))
            .ForMember(m => m.brandshortname, f => f.MapFrom(w => w.BrandShortName))
            .ForMember(m => m.description, f => f.MapFrom(w => w.Description));
    }
}
