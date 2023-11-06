using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.AdjustItemTypeService.Queries.GetAdjustItemTypeByID.v1;
public class GetAdjustItemTypeByIDMappingProfile : Profile
{
    public GetAdjustItemTypeByIDMappingProfile()
    {
        CreateMap<TMAdjustItemType, GetAdjustItemTypeByIDResponseDTO>()
            .ForMember(w => w.adjusttypeid, f => f.MapFrom(w => w.AdjustTypeID))
            .ForMember(w => w.adjusttypename, f => f.MapFrom(w => w.AdjustTypeName))
            .ForMember(w => w.description, f => f.MapFrom(w => w.Description))
            .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(w => w.createddate, f => f.MapFrom(w => w.CreatedDate))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
    }
}
