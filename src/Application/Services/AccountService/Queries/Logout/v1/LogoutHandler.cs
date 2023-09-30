using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMUsers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.AccountService.Queries.Logout.v1;
public class LogoutHandler : BaseService, IRequestHandler<LogoutQuery, BaseResponse<CommandResponse>>
{
    public LogoutHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(LogoutQuery request, CancellationToken cancellationToken)
    {
		TMUsers resUser = await _unitOfWork.Repository<TMUsers>().FirstOrDefaultAsync(w => w.UserName == request.username);
        if(resUser == null)
        {
			return new BaseResponse<CommandResponse>
			{
				result = true,
				data = new CommandResponse { result = false },
				message = "Failed",
				soruce = "db",
				status = StatusCodes.Status203NonAuthoritative.ToString()
			};
		}

		#region Update Logout datetime
		resUser.SetLastLogoutTime();
		resUser.AddDomainEvent(new TMUsersUpdateEvent(resUser));
		await _unitOfWork.SaveChangesAsync();
		#endregion

		return new BaseResponse<CommandResponse>
		{
			result = true,
			data = new CommandResponse { result = true },
			message = "Success",
			soruce = "db",
			status = StatusCodes.Status200OK.ToString()
		};
	}
}
