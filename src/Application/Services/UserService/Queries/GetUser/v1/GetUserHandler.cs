using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployee.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;
public class GetUserHandler : BaseService, IRequestHandler<GetUserQuery, BaseResponse<List<GetUserResponseDTO>>>
{
    public GetUserHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<List<GetUserResponseDTO>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        List<GetUserResponseDTO> resUsers = (from a in await _unitOfWork.Repository<TMUsers>().QueryAsync()
                                             join b in await _unitOfWork.Repository<TMRole>().QueryAsync() on a.RoleID equals b.RoleID
                                             join c in await _unitOfWork.Repository<TMApproveStatus>().QueryAsync() on a.ApproveStatus equals c.ApproveStatusID
                                             where a.IsActive && a.ApproveStatus == (int)ApproveStatus.Approve
                                             select new GetUserResponseDTO
                                             {
                                                 userid = a.UserID,
                                                 username = a.UserName,
                                                 roleid = a.RoleID,
                                                 rolename = b.Name,
                                                 profilepicture = a.ProfilePicture,
                                                 lastlogin = a.LastLogin,
                                                 lastlogout = a.LastLogout,
                                                 createdby = a.CreatedBy,
                                                 creadeddate = a.CreadedDate,
                                                 isactive = a.IsActive,
                                                 approvestatus = a.ApproveStatus,
                                                 approvestatusname = c.ApproveStatusName_TH
                                             }).ToList();

        if (!resUsers.Any())
        {
            throw new Exception("ไม่พบข้อมูลบัญชีผู้ใช้งาน");
        }
        return new BaseResponse<List<GetUserResponseDTO>>
        {
            result = true,
            data = resUsers.OrderBy(w => w.userid).ToList(),
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
