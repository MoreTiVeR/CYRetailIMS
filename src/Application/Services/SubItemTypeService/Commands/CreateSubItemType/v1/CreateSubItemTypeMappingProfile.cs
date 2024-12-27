using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
public class CreateSubItemTypeMappingProfile : Profile
{
    public CreateSubItemTypeMappingProfile()
    {
        CreateMap<CreateSubItemTypeDetail, TMSubItemType>()
            .ForMember(s => s.SubItemCode, f => f.MapFrom(s => s.subitemcode))
            .ForMember(s => s.SubTypeNameTH, f => f.MapFrom(s => s.subtypename_th))
            .ForMember(s => s.SubTypeNameEN, f => f.MapFrom(s => s.subTypename_en))
            .ForMember(s => s.Description, f => f.MapFrom(s => s.description))
            .ForMember(s => s.CreatedBy, f => f.MapFrom(s => s.createdby));
    }
}
