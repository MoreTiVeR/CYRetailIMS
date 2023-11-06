using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Confiuration;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMEmployees;
using CYRetailIMS.Domain.Events.TMUserInBranchs;
using CYRetailIMS.Domain.Events.TMUsers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
public class CreateUserHandler : BaseService, IRequestHandler<CreateUserCommand, BaseResponse<CommandResponse>>
{
	private readonly IAppConfig _appConfig;
	public CreateUserHandler(IMapper mapper, IUnitOfWork unitOfWork,
		IAppConfig appConfig) : base(mapper, unitOfWork)
	{
		_appConfig = appConfig;
	}

	public async Task<BaseResponse<CommandResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		TMEmployee resEmp = await _unitOfWork.Repository<TMEmployee>().FirstOrDefaultAsync(w => w.EmpID == request.empid);
		if (resEmp == null)
		{
			throw new Exception("ไม่สามารถทำรายการได้ เนื่องจากไม่พบข้อมูลพนักงานในระบบ");
		}

		if(resEmp.UserID > 0)
		{
			throw new Exception("ไม่สามารถทำรายการได้ เนื่องจากพนักงานได้ลงทะเบียนใช้งานระบบแล้ว");
		}

		IEnumerable<TMUsers> isExistUser = await _unitOfWork.Repository<TMUsers>().QueryAsync(w => w.UserName.ToLower() == request.username.Trim().ToLower());
		if (isExistUser.Any())
		{
			throw new Exception($"ไม่สามารถทำรายการได้ เนื่องจากมีชื่อผู้ใช้งาน {request.username} ในระบบแล้ว");
        }

		#region Create TMUser
		TMUsers userEnt = CreateUserData(request);
		userEnt.AddDomainEvent(new TMUsersCreateEvent(userEnt));
		#endregion

		#region Create TMUserInBranch
		TMUserInBranch userInBranchEnt = new TMUserInBranch();
		userEnt.TMUserInBranches = CreateUserInBranchData(request);
		userEnt.TMUserInBranches.ToList().ForEach(e =>
		{
			e.AddDomainEvent(new TMUserInBranchCreateEvent(e));
		});
		#endregion

		#region Add User Entity
		await _unitOfWork.Repository<TMUsers>().AddAsync(userEnt);
		await _unitOfWork.SaveChangesAsync();
		#endregion

		#region Update UserID in TMEmployee
		resEmp.UserID = userEnt.UserID;
		resEmp.AddDomainEvent(new TMEmployeeUpdateEvent(resEmp));
		resEmp.SetUpdatedBy(request.createdby);
		resEmp.SetUpdatedDate(request.createddate);
		await _unitOfWork.SaveChangesAsync();
		#endregion

		return new BaseResponse<CommandResponse>
		{
			result = true,
			data = new CommandResponse { result = true },
			status = StatusCodes.Status200OK.ToString(),
			message = "Success",
			soruce = "db"
		};
	}

	private TMUsers CreateUserData(CreateUserCommand request)
	{
		string secretKey = _appConfig.GetUserSecretKey();
		byte[] bytePass = $"{request.username.Trim()}{secretKey}{request.password.Trim()}".ToMD5Password();
		TMUsers userData = new TMUsers
		{
			RoleID = request.roleid,
			UserName = request.username.Trim(),
			Password = bytePass,
			ProfilePicture = request.profilepicture,
			CreatedBy = request.createdby,
			CreatedDate = request.createddate,
			IsActive = request.isactive,
			ApproveStatus = request.approvestatus
		};
		return userData;
	}

	private ICollection<TMUserInBranch> CreateUserInBranchData(CreateUserCommand request)
	{
		List<TMUserInBranch> usersInBranch = new List<TMUserInBranch>();
		usersInBranch.Add(new TMUserInBranch
		{
			BranchID = request.userinbranchid,
			CreatedBy = request.createdby,
			CreatedDate = request.createddate,
			IsActive = true
		});
		return usersInBranch;
	}
}
