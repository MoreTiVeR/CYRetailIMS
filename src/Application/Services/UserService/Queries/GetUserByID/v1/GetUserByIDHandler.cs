using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.UserService.Queries.GetUser.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.UserService.Queries.GetUserByID.v1;
public class GetUserByIDHandler : BaseService, IRequestHandler<GetUserByIDQuery, BaseResponse<GetUserResponseDTO>>
{
    public GetUserByIDHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<GetUserResponseDTO>> Handle(GetUserByIDQuery request, CancellationToken cancellationToken)
    {
        GetUserResponseDTO resUsers = (from a in await _unitOfWork.Repository<TMUsers>().QueryAsync()
                                       join b in await _unitOfWork.Repository<TMRole>().QueryAsync() on a.RoleID equals b.RoleID
                                       join c in await _unitOfWork.Repository<TMApproveStatus>().QueryAsync() on a.ApproveStatus equals c.ApproveStatusID
                                       where a.UserID == request.userid 
                                       && a.IsActive && a.ApproveStatus == (int)ApproveStatus.Approve
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
                                       }).FirstOrDefault();

        if (resUsers == null)
        {
            throw new Exception("ไม่พบข้อมูลบัญชีผู้ใช้งาน");
        }
        return new BaseResponse<GetUserResponseDTO>
        {
            result = true,
            data = resUsers,
            message = " Success",
            soruce = "db",
            status = StatusCodes.Status200OK.ToString()
        };
    }
}
