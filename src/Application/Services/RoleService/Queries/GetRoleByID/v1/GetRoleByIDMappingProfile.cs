using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.RoleService.Queries.GetRoles.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
public class GetRoleByIDMappingProfile : Profile
{
    public GetRoleByIDMappingProfile()
    {
        CreateMap<TMRole, GetRoleByIDResponseDTO>()
                    .ForMember(w => w.roleid, f => f.MapFrom(w => w.RoleID))
                    .ForMember(w => w.name, f => f.MapFrom(w => w.Name))
                    .ForMember(w => w.description, f => f.MapFrom(w => w.Description))
                    .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
                    .ForMember(w => w.createddate, f => f.MapFrom(w => w.CreatedDate))
                    .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
    }
}
