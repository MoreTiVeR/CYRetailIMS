using AutoMapper;
using CYRetailIMS.Application.Common.Models.UI;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;

namespace CYRetailIMS.ComponentService.Web.Common.Mappings.Account;

public class AcountMappingProfile : Profile
{
    public AcountMappingProfile()
    {
        CreateMap<UserProfileResponseDTO, UserProfileViewModel>();
    }
}
