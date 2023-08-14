using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureList.v1;
public class GetUnitOfMeasureListMappingProfile : Profile
{
    public GetUnitOfMeasureListMappingProfile()
    {
        CreateMap<TMUnitOfMeasure, GetUnitOfMeasureListResponseDTO>()
            .ForMember(m => m.unitofmeasureid, f => f.MapFrom(x => x.UnitOfMeasureID))
            .ForMember(m => m.unitofmeasurename, f => f.MapFrom(x => x.UnitOfMeasureName))
            .ForMember(m => m.description, f => f.MapFrom(x => x.Description));
    }
}
