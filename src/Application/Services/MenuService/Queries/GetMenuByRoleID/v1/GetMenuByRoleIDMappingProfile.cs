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
            .ForMember(m => m.MenuID, f => f.MapFrom(x => x.Menu.MenuID))
            .ForMember(m => m.Seq, f => f.MapFrom(x => x.Menu.Seq))
            .ForMember(m => m.MenuName_TH, f => f.MapFrom(x => x.Menu.MenuName_TH))
            .ForMember(m => m.MenuName_EN, f => f.MapFrom(x => x.Menu.MenuName_EN))
            .ForMember(m => m.CMS_DataIconName, f => f.MapFrom(x => x.Menu.CMS_DataIconName))
            .ForMember(m => m.CMS_Link, f => f.MapFrom(x => x.Menu.CMS_Link))
            .ForMember(m => m.CMS_Title, f => f.MapFrom(x => x.Menu.CMS_Title))
            .ForMember(m => m.IsActive, f => f.MapFrom(x => x.Menu.IsActive));

            //.AfterMap((s, d) =>
            //{
            //    new List<SubMenuResponseDTO>().Add(new SubMenuResponseDTO
            //    {
            //        SubMenuID = s.SubMenu.SubMenuID,
            //        Seq = s.SubMenu.Seq
            //    });
            //});
    }
}
