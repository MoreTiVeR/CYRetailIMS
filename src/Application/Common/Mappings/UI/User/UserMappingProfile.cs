using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
using CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;
using CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;

namespace CYRetailIMS.Application.Common.Mappings.UI.User;
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserViewModel, CreateUserCommand>()
            .ForMember(w => w.username, f => f.MapFrom(w => w.UserName))
            .ForMember(w => w.password, f => f.MapFrom(w => w.Password))
            .ForMember(w => w.roleid, f => f.MapFrom(w => w.RoleID))
            .ForMember(w => w.userinbranchid, f => f.MapFrom(w => w.BranchID))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive))
            .ForMember(w => w.createdby, f => f.MapFrom(w => w.CreatedBy))
            .ForMember(w => w.createddate, f => f.MapFrom(w => w.CreatedDate));

        CreateMap<EditUserViewModel, UpdateUserCommand>()
            .ForMember(w => w.userid, f => f.MapFrom(w => w.UserID))
            .ForMember(w => w.password, f => f.MapFrom(w => w.Password))
            .ForMember(w => w.roleid, f => f.MapFrom(w => w.RoleID))
            .ForMember(w => w.userinbranchid, f => f.MapFrom(w => w.BranchID))
            .ForMember(w => w.updatedby, f => f.MapFrom(w => w.UpdatedBy))
            .ForMember(w => w.updateddate, f => f.MapFrom(w => w.UpdatedDate))
            .ForMember(w => w.isactive, f => f.MapFrom(w => w.IsActive));

        CreateMap<GetUserResponseDTO, EditUserViewModel>()
            .ForMember(w => w.UserID, f => f.MapFrom(w => w.userid))
            .ForMember(w => w.RoleID, f => f.MapFrom(w => w.roleid))
            .ForMember(w => w.BranchID, f => f.MapFrom(w => w.branchid))
            .ForMember(w => w.IsActive, f => f.MapFrom(w => w.isactive));

    }
    
}
