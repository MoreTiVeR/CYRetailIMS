using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
public class GetMenuByRoleIDHandler : BaseService, IRequestHandler<GetMenuByRoleIDQuery, BaseResponse<List<GetMenuByRoleIDResponseDTO>>>
{
    public GetMenuByRoleIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetMenuByRoleIDResponseDTO>>> Handle(GetMenuByRoleIDQuery request, CancellationToken cancellationToken)
    {
        #region Get RoleMenu Data
        IQueryable<TMRoleInMenu> resMenu = await _unitOfWork.Repository<TMRoleInMenu>()
                    .FindWithInclude(w => w.RoleID == request.roleid, x => x.Include(ss => ss.Menu), x2 => x2.Include(ss2 => ss2.SubMenu));
        resMenu = resMenu.Where(w => w.Menu.IsActive == true && w.SubMenu.IsActive == true);
        if (resMenu?.Count() == 0)
        {
            throw new Exception("Data Not Found");
        }

        List<GetMenuByRoleIDResponseDTO> resMapiing = (from menu in resMenu.ToList()
                                                       group menu by menu.MenuID into s
                                                       select new GetMenuByRoleIDResponseDTO
                                                       {
                                                           menuid = s.Key,
                                                           seq = s.FirstOrDefault(w => w.MenuID == s.Key).Menu.Seq,
                                                           menuname_th = s.FirstOrDefault(w => w.MenuID == s.Key).Menu.MenuName_TH,
                                                           menuname_en = s.FirstOrDefault(w => w.MenuID == s.Key).Menu.MenuName_EN,
                                                           description = s.FirstOrDefault(w => w.MenuID == s.Key).Menu.Description,
                                                           cms_icon_name = s.FirstOrDefault(w => w.MenuID == s.Key).Menu.CMS_DataIconName,
                                                           cms_link = s.FirstOrDefault(w => w.MenuID == s.Key).Menu.CMS_Link,
                                                           cms_title = s.FirstOrDefault(w => w.MenuID == s.Key).Menu.CMS_Title,
                                                           isactive = s.FirstOrDefault(w => w.MenuID == s.Key).Menu.IsActive,
                                                           submenulist = s.Select(s => new SubMenuResponseDTO
                                                           {
                                                               submenuid = s.SubMenuID,
                                                               seq = s.SubMenu.Seq,
                                                               menuname_th = s.SubMenu.MenuName_TH,
                                                               menuname_en = s.SubMenu.MenuName_EN,
                                                               description = s.SubMenu.Description,
                                                               cms_controllername = s.SubMenu.CMS_ControllerName,
                                                               cms_actionname = s.SubMenu.CMS_ActionName,
                                                               cms_i_class = s.SubMenu.CMS_I_Class,
                                                               cms_span_class = s.SubMenu.CMS_Span_Class,
                                                               cms_link = s.SubMenu.CMS_Link,
                                                               isactive = s.SubMenu.IsActive

                                                           }).ToList()
                                                       }).ToList();
        #endregion

        return new BaseResponse<List<GetMenuByRoleIDResponseDTO>>
        {
            result = true,
            data = resMapiing,
            message = "Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
