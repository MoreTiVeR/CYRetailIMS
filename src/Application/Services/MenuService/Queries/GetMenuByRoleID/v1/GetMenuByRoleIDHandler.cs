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
		#region Option#1
		//Stopwatch sw = new Stopwatch();
		//sw.Start();
		//var resMenux = (from a in await _unitOfWork.Repository<TMRoleInMenu>().QueryAsync()
		//				join menu in await _unitOfWork.Repository<TMMenus>().QueryAsync() on a.MenuID equals menu.MenuID
		//				join submenu in await _unitOfWork.Repository<TMSubMenus>().QueryAsync() on new { a.MenuID, a.SubMenuID } equals new { submenu.MenuID, submenu.SubMenuID }
		//				where a.RoleID == request.roleid 
		//				&& menu.IsActive && submenu.IsActive
		//				group new { menu, submenu } by new { a.MenuID } into grps
		//				select new GetMenuByRoleIDResponseDTO
		//				{
		//					menuid = grps.Key.MenuID,
		//					seq = grps.FirstOrDefault(w => w.menu.MenuID == grps.Key.MenuID).menu.Seq,
		//					menuname_th = grps.FirstOrDefault(w => w.menu.MenuID == grps.Key.MenuID).menu.MenuName_TH,
		//					menuname_en = grps.FirstOrDefault(w => w.menu.MenuID == grps.Key.MenuID).menu.MenuName_EN,
		//					cms_icon_name = grps.FirstOrDefault(w => w.menu.MenuID == grps.Key.MenuID).menu.CMS_DataIconName,
		//					cms_link = grps.FirstOrDefault(w => w.menu.MenuID == grps.Key.MenuID).menu.CMS_Link,
		//					cms_title = grps.FirstOrDefault(w => w.menu.MenuID == grps.Key.MenuID).menu.CMS_Title,
		//					description = grps.FirstOrDefault(w => w.menu.MenuID == grps.Key.MenuID).menu.Description,
		//					isactive = grps.FirstOrDefault(w => w.menu.MenuID == grps.Key.MenuID).menu.IsActive,
		//					submenulist = (from sdata in  grps.Where(w => w.menu.MenuID == grps.Key.MenuID)
		//								   let submenu_data = sdata.submenu
		//								   select new SubMenuResponseDTO
		//								   {
		//									   submenuid = submenu_data.SubMenuID,
		//									   seq = submenu_data.Seq,
		//									   menuname_th = submenu_data.MenuName_TH,
		//									   menuname_en = submenu_data.MenuName_EN,
		//									   description = submenu_data.Description,
		//									   cms_controllername = submenu_data.CMS_ControllerName,
		//									   cms_actionname = submenu_data.CMS_ActionName,
		//									   cms_i_class = submenu_data.CMS_I_Class,
		//									   cms_span_class = submenu_data.CMS_Span_Class,
		//									   cms_link = submenu_data.CMS_Link,
		//									   isactive = submenu_data.IsActive
		//								   }).OrderBy(o => o.seq).ToList()
		//				}).OrderBy(o => o.seq).ToList();
		//sw.Stop();
		//TimeSpan sp = new TimeSpan(sw.ElapsedMilliseconds);
		//var ss = sp.TotalMilliseconds;
		#endregion

		#region Get RoleMenu Data
		IQueryable<TMRoleInMenu> resMenu = await _unitOfWork.Repository<TMRoleInMenu>()
					.FindWithInclude(w => w.RoleID == request.roleid, x => x.Include(ss => ss.Menu), x2 => x2.Include(ss2 => ss2.SubMenu));
		resMenu = resMenu.Where(w => w.Menu.IsActive == true && w.SubMenu.IsActive == true);

		//test
		//var dd = resMenu.Where(w => w.Menu.IsActive == true && w.SubMenu.IsActive == true).GroupBy(g => g.MenuID).ToList();
		//var _dd = dd.ToList();

		if (resMenu?.Count() == 0)
		{
			throw new Exception("Data Not Found");
		}
		List<GetMenuByRoleIDResponseDTO> resMapiing = _mapper.Map<List<TMRoleInMenu>, List<GetMenuByRoleIDResponseDTO>>(resMenu.ToList())
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
							   }).OrderBy(o => o.seq).ToList()
			}).OrderBy(o => o.seq).ToList();
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
