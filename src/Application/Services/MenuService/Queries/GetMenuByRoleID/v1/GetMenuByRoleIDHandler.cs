using System;
using System.Collections.Generic;
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
        #region Option#1
        //var resx = (from a in await _unitOfWork.Repository<TMMenus>().QueryAsync()
        //            join b in await _unitOfWork.Repository<TMSubMenus>().QueryAsync() on a.MenuID equals b.MenuID
        //            join c in await _unitOfWork.Repository<TMRoleInMenus>().QueryAsync() on new { a.MenuID, b.SubMenuID } equals new { c.MenuID, c.SubMenuID }
        //            let listx = b
        //            where c.RoleID == request.RoleID && a.IsActive
        //            select new GetMenuByRoleIDResponseDTO
        //            {
        //                MenuID = a.MenuID,
        //                Seq = a.Seq,
        //                MenuName_TH = a.MenuName_TH,
        //                MenuName_EN = a.MenuName_EN,
        //                CMS_DataIconName = a.CMS_DataIconName,
        //                CMS_Link = a.CMS_Link,
        //                CMS_Title = a.CMS_Title,
        //                Description = a.Description,
        //                IsActive = a.IsActive,
        //                SubMenuList = new List<SubMenuResponseDTO>() { new SubMenuResponseDTO { SubMenuID = b.SubMenuID, Seq = b.Seq } }
        //            }).ToList();
        //var _resx = resx;
        #endregion

        #region Option#2
        IQueryable<TMRoleInMenus> resMenu = await _unitOfWork.Repository<TMRoleInMenus>()
            .FindWithInclude(w => w.RoleID == request.RoleID, x => x.Include(ss => ss.Menu), x2 => x2.Include(ss2 => ss2.SubMenu));

        if (resMenu?.Count() == 0)
        {
            throw new Exception("Data Not Found");
        }
        List<GetMenuByRoleIDResponseDTO> resMapiing = _mapper.Map<List<TMRoleInMenus>, List<GetMenuByRoleIDResponseDTO>>(resMenu.ToList())
            .GroupBy(g => g.menuid)
            .Select(s => new GetMenuByRoleIDResponseDTO
            {
                menuid = s.Key,
                seq = s.FirstOrDefault(w => w.menuid == s.Key).seq,
                menuname_th = s.FirstOrDefault(w => w.menuid == s.Key).menuname_th,
                menuname_en = s.FirstOrDefault(w => w.menuid == s.Key).menuname_en,
                description = s.FirstOrDefault(w => w.menuid == s.Key).description,
				cms_icon_name = s.FirstOrDefault(w => w.menuid == s.Key).cms_icon_name,
                cms_link = s.FirstOrDefault(w => w.menuid == s.Key).cms_link,
                cms_title = s.FirstOrDefault(w => w.menuid == s.Key).cms_title,
                isactive = s.FirstOrDefault(w => w.menuid == s.Key).isactive,
                submenulist = (from a in resMenu.Select(s => s.SubMenu)
                               where a.MenuID == s.Key && a.IsActive
                               select new SubMenuResponseDTO
                               {
                                   submenuid = a.SubMenuID,
                                   seq = a.Seq,
                                   menuname_th = a.MenuName_TH,
                                   menuname_en = a.MenuName_EN,
                                   description = a.Description,
                                   cms_controllername = a.CMS_ControllerName,
                                   cms_actionname = a.CMS_ActionName,
                                   cms_i_class = a.CMS_I_Class,
                                   cms_span_class = a.CMS_Span_Class,
                                   cms_link = a.CMS_Link,
                                   isactive = a.IsActive
                               }).ToList()
            }).Where(w => w.isactive).ToList();
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
