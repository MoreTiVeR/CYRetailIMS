using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;

namespace CYRetailIMS.Application.Common.Mappings.UI.Account;
public class AcountMappingProfile : Profile
{
	public AcountMappingProfile()
	{
		CreateMap<UserProfileResponseDTO, UserProfileViewModel>();
	}
}
