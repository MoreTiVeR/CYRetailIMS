using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemType.v1;
public class GetAdjustItemTypeMappingProfile : Profile
{
    public GetAdjustItemTypeMappingProfile()
    {
        CreateMap<TMAdjustItemType, GetAdjustItemTypeResposeDTO>()
            .ForMember(w => w.adjusttypeid, f => f.MapFrom(w => w.AdjustTypeID))
            .ForMember(w => w.adjusttypename, f => f.MapFrom(w => w.AdjustTypeName))
            .ForMember(w => w.description, f => f.MapFrom(w => w.Description))
            .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(w => w.creadeddate, f => f.MapFrom(w => w.CreadedDate))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
    }
}
