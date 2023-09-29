using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using MediatR;

namespace CYRetailIMS.Application.Common.Mappings.UI.Employee;
public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<CreateEmployeeViewModel, CreateEmployeeCommand>()
            .ForMember(w => w.departmentid, f => f.MapFrom(w => w.DepartmentID))
            .ForMember(w => w.firstname, f => f.MapFrom(w => w.FirstName))
            .ForMember(w => w.lastname, f => f.MapFrom(w => w.LastName))
            .ForMember(w => w.email, f => f.MapFrom(w => w.Email))
            .ForMember(w => w.mobileno, f => f.MapFrom(w => w.MobileNo))
            .ForMember(w => w.nickname, f => f.MapFrom(w => w.NickName))
            .ForMember(w => w.IsActive, f => f.MapFrom(w => w.IsActive))
            .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(w => w.creadeddate, f => f.MapFrom(w => w.CreatedDate));


        CreateMap<GetEmployeeResponseDTO, EditEmployeeViewModel>()
            .ForMember(w => w.EmpID, f => f.MapFrom(w => w.empid))
            .ForMember(w => w.DepartmentID, f => f.MapFrom(w => w.departmentid))
            .ForMember(w => w.FirstName, f => f.MapFrom(w => w.firstname))
            .ForMember(w => w.LastName, f => f.MapFrom(w => w.lastname))
            .ForMember(w => w.NickName, f => f.MapFrom(w => w.nickname))
            .ForMember(w => w.Email, f => f.MapFrom(w => w.email))
            .ForMember(w => w.MobileNo, f => f.MapFrom(w => w.mobileno))
            .ForMember(w => w.IsActive, f => f.MapFrom(w => w.isactive));

        CreateMap<EditEmployeeViewModel, UpdateEmployeeCommand>()
            .ForMember(w => w.empid, f => f.MapFrom(w => w.EmpID))
            .ForMember(w => w.departmentid, f => f.MapFrom(w => w.DepartmentID))
            .ForMember(w => w.firstname, f => f.MapFrom(w => w.FirstName))
            .ForMember(w => w.lastname, f => f.MapFrom(w => w.LastName))
            .ForMember(w => w.nickname, f => f.MapFrom(w => w.NickName))
            .ForMember(w => w.email, f => f.MapFrom(w => w.Email))
            .ForMember(w => w.mobileno, f => f.MapFrom(w => w.MobileNo))
            .ForMember(w => w.updatedby, f => f.MapFrom(w => w.UpdatedBy))
            .ForMember(w => w.updateddate, f => f.MapFrom(w => w.UpdatedDate))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));

    }
}
