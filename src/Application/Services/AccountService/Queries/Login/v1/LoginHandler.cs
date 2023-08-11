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
    public LoginHandler(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration) : base(mapper, unitOfWork)
    {
        _configuration = configuration;
        _getMenuByRoleIDHandler = new GetMenuByRoleIDHandler(_mapper, unitOfWork);
    }

    public async Task<BaseResponse<UserProfileResponseDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        string secretKey = _configuration.GetSection("AppSettings")["SECRET_KEY"];
        byte[] bytePass = $"{request.UserName.Trim().ToLower()}{secretKey}{request.Password}".ToMD5Password();
        IQueryable<TMUsers> resUser = await _unitOfWork.Repository<TMUsers>().FindWithInclude(w => w.UserName == request.UserName && w.Password == bytePass, i => i.Include(x => x.TMEmployees));
        if(resUser?.Count() == 0)
        {
            throw new Exception("ชื่อผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง");
        }

        UserProfileResponseDTO resData = (from a in resUser
                                          select new UserProfileResponseDTO
                                          {
                                              UserID = a.UserID,
                                              RoleID = a.RoleID,
                                              UserName = a.UserName,
                                              Email = a.TMEmployees.FirstOrDefault().Email,
                                              FirstName = a.TMEmployees.FirstOrDefault().FirstName,
                                              LastName = a.TMEmployees.FirstOrDefault().LastName,
                                              ProfilePicture = a.ProfilePicture,
                                              LastLogout = a.LastLogout,
                                              IsActive = a.IsActive,
                                              ApproveStatus = a.ApproveStatus,
                                              access_menu = new List<GetMenuByRoleIDResponseDTO>()
                                          }).FirstOrDefault();

        BaseResponse<List<GetMenuByRoleIDResponseDTO>> resMenu = await _getMenuByRoleIDHandler.Handle(new GetMenuByRoleIDQuery { RoleID = resUser.FirstOrDefault().RoleID }, CancellationToken.None);
        if (resMenu.Result)
        {
            resData.access_menu = resMenu.Data;
        }
        return new BaseResponse<UserProfileResponseDTO>
        {
            Result = true,
            Data = resData,
            Message = "Sucess",
            Soruce = "db",
            Status = StatusCodes.Status200OK.ToString()
        };
    }
}
