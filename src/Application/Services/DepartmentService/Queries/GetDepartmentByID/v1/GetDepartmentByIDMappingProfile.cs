using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartments.v1;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;
public class GetDepartmentByIDMappingProfile : Profile
{
    public GetDepartmentByIDMappingProfile()
    {
        CreateMap<TMDepartment, GetDepartmentByIDResponseDTO>()
            .ForMember(w => w.departmentid, f => f.MapFrom(w => w.DepartmentID))
            .ForMember(w => w.departmentname, f => f.MapFrom(w => w.DepartmentName))
            .ForMember(w => w.description, f => f.MapFrom(w => w.Description))
            .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(w => w.creadeddate, f => f.MapFrom(w => w.CreadedDate))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));
    }
}
