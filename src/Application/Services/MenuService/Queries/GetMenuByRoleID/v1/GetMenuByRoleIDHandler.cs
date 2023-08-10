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
        var resx = (from a in await _unitOfWork.Repository<TMMenus>().QueryAsync()
                    join b in await _unitOfWork.Repository<TMSubMenus>().QueryAsync() on a.MenuID equals b.MenuID
                    join c in await _unitOfWork.Repository<TMRoleInMenus>().QueryAsync() on new { a.MenuID, b.SubMenuID } equals new { c.MenuID, c.SubMenuID }
                    let listx = b
                    where c.RoleID == request.RoleID && a.IsActive
                    select new GetMenuByRoleIDResponseDTO
                    {
                        MenuID = a.MenuID,
                        Seq = a.Seq,
                        MenuName_TH = a.MenuName_TH,
                        MenuName_EN = a.MenuName_EN,
                        CMS_DataIconName = a.CMS_DataIconName,
                        CMS_Link = a.CMS_Link,
                        CMS_Title = a.CMS_Title,
                        Description = a.Description,
                        IsActive = a.IsActive,
                        SubMenuList = new List<SubMenuResponseDTO>() { new SubMenuResponseDTO { SubMenuID = b.SubMenuID, Seq = b.Seq } }
                    }).ToList();
        var _resx = resx;
        #endregion

        #region Option#2
        IQueryable<TMRoleInMenus> resMenu = await _unitOfWork.Repository<TMRoleInMenus>()
            .FindWithInclude(w => w.RoleID == request.RoleID, x => x.Include(ss => ss.Menu), x2 => x2.Include(ss2 => ss2.SubMenu));

        if (resMenu?.Count() == 0)
        {
            throw new Exception("Data Not Found");
        }
        List<GetMenuByRoleIDResponseDTO> resMapiing = _mapper.Map<List<TMRoleInMenus>, List<GetMenuByRoleIDResponseDTO>>(resMenu.ToList())
            .GroupBy(g => g.MenuID)
            .Select(s => new GetMenuByRoleIDResponseDTO
            {
                MenuID = s.Key,
                Seq = s.FirstOrDefault(w => w.MenuID == s.Key).Seq,
                MenuName_TH = s.FirstOrDefault(w => w.MenuID == s.Key).MenuName_TH,
                MenuName_EN = s.FirstOrDefault(w => w.MenuID == s.Key).MenuName_EN,
                Description = s.FirstOrDefault(w => w.MenuID == s.Key).Description,
                CMS_DataIconName = s.FirstOrDefault(w => w.MenuID == s.Key).CMS_DataIconName,
                CMS_Link = s.FirstOrDefault(w => w.MenuID == s.Key).CMS_Link,
                CMS_Title = s.FirstOrDefault(w => w.MenuID == s.Key).CMS_Title,
                IsActive = s.FirstOrDefault(w => w.MenuID == s.Key).IsActive,
                SubMenuList = (from a in resMenu.Select(s => s.SubMenu)
                               where a.MenuID == s.Key && a.IsActive
                               select new SubMenuResponseDTO
                               {
                                   SubMenuID = a.SubMenuID,
                                   Seq = a.Seq,
                                   MenuName_TH = a.MenuName_TH,
                                   MenuName_EN = a.MenuName_EN,
                                   Description = a.Description,
                                   CMS_ControllerName = a.CMS_ControllerName,
                                   CMS_ActionName = a.CMS_ActionName,
                                   CMS_I_Class = a.CMS_I_Class,
                                   CMS_Span_Class = a.CMS_Span_Class,
                                   CMS_Link = a.CMS_Link,
                                   IsActive = a.IsActive
                               }).ToList()
            }).Where(w => w.IsActive).ToList();
        #endregion

        return new BaseResponse<List<GetMenuByRoleIDResponseDTO>>
        {
            Result = true,
            Data = resMapiing,
            Message = "Success",
            Soruce = "db",
            Status = StatusCodes.Status200OK.ToString()
        };
    }
}
