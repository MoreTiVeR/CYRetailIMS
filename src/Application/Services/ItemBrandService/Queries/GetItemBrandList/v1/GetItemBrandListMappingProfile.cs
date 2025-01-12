using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandByID.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandList.v1;
internal class GetItemBrandListMappingProfile : Profile
{
    public GetItemBrandListMappingProfile()
    {
        CreateMap<TMItemBrand, GetItemBrandListResponseDTO>()
            .ForMember(m => m.brandid, f => f.MapFrom(w => w.BrandID))
            .ForMember(m => m.brandname, f => f.MapFrom(w => w.BrandName))
            .ForMember(m => m.brandshortname, f => f.MapFrom(w => w.BrandShortName))
            .ForMember(m => m.description, f => f.MapFrom(w => w.Description))
            .ForMember(m => m.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(m => m.createddate, f => f.MapFrom(w => w.CreatedDate))
            .ForMember(m => m.updatedby, f => f.MapFrom(w => w.UpdatedBy))
            .ForMember(m => m.updateddate, f => f.MapFrom(w => w.UpdatedDate))
            .ForMember(m => m.isactive, f => f.MapFrom(w => w.IsActive));
    }
}
