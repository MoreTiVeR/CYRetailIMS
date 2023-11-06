using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
public class CreateUserMappingProfile : Profile
{
	public CreateUserMappingProfile()
	{
		CreateMap<CreateUserCommand, TMUsers>()
			.ForMember(w => w.RoleID, f => f.MapFrom(w => w.roleid))
			.ForMember(w => w.UserName, f => f.MapFrom(w => w.username))
			.ForMember(w => w.Password, f => f.MapFrom(w => w.password))
			.ForMember(w => w.ProfilePicture, f => f.MapFrom(w => w.profilepicture))
			.ForMember(w => w.CreatedBy, f => f.MapFrom(w => w.createdby))
			.ForMember(w => w.CreatedDate, f => f.MapFrom(w => w.createddate));
	}
}
