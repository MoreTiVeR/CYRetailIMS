using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeList.v1;
public class GetItemTypeListMappingProfile : Profile
{
    public GetItemTypeListMappingProfile()
    {
        CreateMap<TMItemType, GetItemTypeListResponseDTO>()
            .ForMember(m => m.itemtypeid, f => f.MapFrom(ff => ff.ItemTypeID))
            .ForMember(m => m.itemtypename, f => f.MapFrom(ff => ff.ItemTypeName))
            .ForMember(m => m.description, f => f.MapFrom(ff => ff.Description));
    }
}
