using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using CYRetailIMS.Application.Common.Extensions;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMEmployees;
using CYRetailIMS.Domain.Events.TMUserInBranchs;
using CYRetailIMS.Domain.Events.TMUsers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
public class CreateEmployeeHandler : BaseService, IRequestHandler<CreateEmployeeCommand, BaseResponse<CommandResponse>>
{
	private readonly IConfiguration _configuration;
	public CreateEmployeeHandler(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration) : base(mapper, unitOfWork)
	{
		_configuration = configuration;
	}

	public async Task<BaseResponse<CommandResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
	{
		TMEmployee isExistEmployee = (from a in await _unitOfWork.Repository<TMEmployee>().QueryAsync()
									  where a.Email == request.email 
									  || (a.FirstName.Trim() == request.firstname.Trim() && a.LastName.Trim() == request.lastname.Trim())
									  select a).FirstOrDefault();

		if (isExistEmployee != null)
		{
			throw new Exception("มีพนักงานนี้ในระบบแล้ว กรุณาลองใหม่อีกครั้ง");
		}

		//Create TMEmployee, TMUsers
		TMEmployee empEntity = _mapper.Map<TMEmployee>(request);
		empEntity.ActiveStatus();
		empEntity.SetCreatedDate();
		empEntity.SetCreatedBy();
		empEntity.AddDomainEvent(new TMEmployeeCreateEvent(empEntity));
		_unitOfWork.Repository<TMEmployee>().Add(empEntity);
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
