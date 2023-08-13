using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Domain.Entities;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
public class GetMenuByRoleIDMappingProfile : Profile
{
    public GetMenuByRoleIDMappingProfile()
    {
        CreateMap<TMSubMenus, SubMenuResponseDTO>();
        CreateMap<TMRoleInMenus, GetMenuByRoleIDResponseDTO>()
            .ForMember(m => m.menuid, f => f.MapFrom(x => x.Menu.MenuID))
            .ForMember(m => m.seq, f => f.MapFrom(x => x.Menu.Seq))
            .ForMember(m => m.menuname_th, f => f.MapFrom(x => x.Menu.MenuName_TH))
            .ForMember(m => m.menuname_en, f => f.MapFrom(x => x.Menu.MenuName_EN))
            .ForMember(m => m.cms_icon_name, f => f.MapFrom(x => x.Menu.CMS_DataIconName))
            .ForMember(m => m.cms_link, f => f.MapFrom(x => x.Menu.CMS_Link))
            .ForMember(m => m.cms_title, f => f.MapFrom(x => x.Menu.CMS_Title))
            .ForMember(m => m.isactive, f => f.MapFrom(x => x.Menu.IsActive));
    }
}
