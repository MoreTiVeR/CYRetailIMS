using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMEmployees;
using CYRetailIMS.Domain.Events.TMUsers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.UserService.Commands.DeleteUser.v1;
public class DeleteUserHadler : BaseService, IRequestHandler<DeleteUserCommand, BaseResponse<CommandResponse>>
{
    public DeleteUserHadler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        TMUsers resUser = await _unitOfWork.Repository<TMUsers>().FirstOrDefaultAsync(w => w.UserID == request.userid && w.IsActive);
        if(resUser == null)
        {
            throw new Exception("ไม่พบข้อมูลพนักงาน");
        }
        resUser.DeActiveStatus();
        resUser.SetUpdatedDate(DateTime.Now);
        resUser.SetUpdatedBy(request.updatedby);
        resUser.AddDomainEvent(new TMUsersUpdateEvent(resUser));
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
