using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
using CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
public class LoginHandler : BaseService, IRequestHandler<LoginQuery, BaseResponse<UserProfileResponseDTO>>
{
    private readonly IConfiguration _configuration;
    private readonly GetMenuByRoleIDHandler _getMenuByRoleIDHandler;
    private readonly GetUserInBranchByUserIDHandler _getUserInBranchByUserIDHandler;
	public LoginHandler(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration) : base(mapper, unitOfWork)
    {
        _configuration = configuration;
        _getMenuByRoleIDHandler = new GetMenuByRoleIDHandler(_mapper, unitOfWork);
		_getUserInBranchByUserIDHandler = new GetUserInBranchByUserIDHandler(_mapper, unitOfWork);

	}

    public async Task<BaseResponse<UserProfileResponseDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        string secretKey = _configuration.GetSection("AppSettings")["SECRET_KEY"];
        byte[] bytePass = $"{request.username.Trim().ToLower()}{secretKey}{request.password}".ToMD5Password();
        IQueryable<TMUsers> resUser = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => w.UserName == request.username && w.Password == bytePass && w.IsActive, 
            i => i.Include(x => x.TMEmployees),
            ii => ii.Include(xx => xx.Role));
        if(!resUser.Any())
        {
            throw new Exception("ชื่อผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง");
        }

        UserProfileResponseDTO resData = (from a in resUser
                                          select new UserProfileResponseDTO
                                          {
                                              userid = a.UserID,
                                              roleid = a.RoleID,
                                              rolename = a.Role.Name,
                                              username = a.UserName,
                                              email = a.TMEmployees.FirstOrDefault().Email,
                                              firstname = a.TMEmployees.FirstOrDefault().FirstName,
                                              lastname = a.TMEmployees.FirstOrDefault().LastName,
                                              profilepicture = a.ProfilePicture,
                                              lastlogout = a.LastLogout,
                                              isactive = a.IsActive,
                                              approvestatus = a.ApproveStatus.Value,
                                              access_menu = new List<GetMenuByRoleIDResponseDTO>()
                                          }).FirstOrDefault();


		#region Get Access Menu
		BaseResponse<List<GetMenuByRoleIDResponseDTO>> resMenu = await _getMenuByRoleIDHandler.Handle(new GetMenuByRoleIDQuery { roleid = resData.roleid }, CancellationToken.None);
		if (resMenu.result)
		{
			resData.access_menu = resMenu.data;
		}
        #endregion

        #region Get Access Branch
        BaseResponse<GetUserInBranchByUserIDResponseDTO> resUserBranch = await _getUserInBranchByUserIDHandler.Handle(new GetUserInBranchByUserIDQuery { userid = resData.userid }, CancellationToken.None);
        if (resUserBranch.result)
        {
            resData.access_branch = resUserBranch.data.branchs;
		}
        #endregion

		return new BaseResponse<UserProfileResponseDTO>
        {
            result = true,
            data = resData,
            message = "Sucess",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
