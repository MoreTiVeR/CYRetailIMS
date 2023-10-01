using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Confiuration;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMUserInBranchs;
using CYRetailIMS.Domain.Events.TMUsers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;
public class UpdateUserHandler : BaseService, IRequestHandler<UpdateUserCommand, BaseResponse<CommandResponse>>
{
    private readonly IAppConfig _appConfig;
    public UpdateUserHandler(IMapper mapper, IUnitOfWork unitOfWork, IAppConfig appConfig) : base(mapper, unitOfWork)
    {
        _appConfig = appConfig;
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        TMUsers resUser = await _unitOfWork.Repository<TMUsers>().FirstOrDefaultAsync(w => w.UserID == request.userid);
        if(resUser == null)
        {
            throw new Exception("ไม่พบบัญชีผู้ใช้งาน");
        }

        #region Update TMUser
        resUser.RoleID = request.roleid;
        if (!string.IsNullOrEmpty(request.password))
        {
            string secretKey = _appConfig.GetUserSecretKey();
            byte[] newBytePass = $"{resUser.UserName.Trim()}{secretKey}{request.password.Trim()}".ToMD5Password();
            resUser.Password = newBytePass;
        }
        //resUser.ProfilePicture = request.profilepicture;
        resUser.IsActive = request.isactive;
        resUser.SetUpdatedBy(request.updatedby);
        resUser.SetUpdatedDate(request.updateddate);
        resUser.AddDomainEvent(new TMUsersUpdateEvent(resUser));
        #endregion

        #region Update TMUserInBranchs when deactive
        TMUserInBranch resUserInBranch = await _unitOfWork.Repository<TMUserInBranch>().FirstOrDefaultAsync(w => w.UserID == request.userid);
        if (resUserInBranch != null && (request.isactive != resUserInBranch.IsActive))
        {
            resUserInBranch.BranchID = request.userinbranchid;
            resUserInBranch.IsActive = request.isactive;
            resUserInBranch.SetUpdatedBy(request.updatedby);
            resUserInBranch.SetUpdatedDate(request.updateddate);
            resUserInBranch.AddDomainEvent(new TMUserInBranchUpdateEvent(resUserInBranch));
        }
        #endregion

        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CommandResponse>
        {
            result = true,
            data = new CommandResponse { result = true },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }
}
