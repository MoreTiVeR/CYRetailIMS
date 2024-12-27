using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Queries.GetSubItemTypeList.v1;
public class GetSubItemTypeMappingProfile : Profile
{
    public GetSubItemTypeMappingProfile()
    {
        CreateMap<TMSubItemType, GetSubItemTypeResponseDTO>()
            .ForMember(s => s.subitemtypeid, f => f.MapFrom(s => s.SubItemTypeID))
            .ForMember(s => s.subitemcode, f => f.MapFrom(s => s.SubItemCode))
            .ForMember(s => s.nameth, f => f.MapFrom(s => s.SubTypeNameTH))
            .ForMember(s => s.nameen, f => f.MapFrom(s => s.SubTypeNameEN))
            .ForMember(s => s.description, f => f.MapFrom(s => s.Description))
            .ForMember(s => s.createdby, f => f.MapFrom(s => s.CreatedBy))
            .ForMember(s => s.createddate, f => f.MapFrom(s => s.CreatedDate))
            .ForMember(s => s.updatedby, f => f.MapFrom(s => s.UpdatedBy))
            .ForMember(s => s.updateddate, f => f.MapFrom(s => s.UpdatedDate))
            .ForMember(s => s.isactive, f => f.MapFrom(s => s.IsActive));
    }
}
