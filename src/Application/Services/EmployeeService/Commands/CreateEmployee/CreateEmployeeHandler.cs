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
using CYRetailIMS.Domain.Events.TMUsers;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        TMUser userEntity = CreateUserData(request);
        //userEntity.AddDomainEvent(new TMUsersCreateEvent(userEntity));
        //_unitOfWork.Repository<TMUser>().Add(userEntity);

        TMEmployee empEntity = _mapper.Map<TMEmployee>(request);
        empEntity.ActiveStatus();
        empEntity.SetCreatedDate();
        empEntity.SetCreatedBy();
        empEntity.User = CreateUserData(request);
        empEntity.User.AddDomainEvent(new TMUsersCreateEvent(empEntity.User));
        empEntity.AddDomainEvent(new TMEmployeeCreateEvent(empEntity));
        _unitOfWork.Repository<TMEmployee>().Add(empEntity);

        await _unitOfWork.SaveChangesAsync();

        return new BaseResponse<CommandResponse>
        {
            Result = true,
            Data = new CommandResponse { result = true },
            Status = StatusCodes.Status200OK.ToString(),
            Message = "Success",
            Soruce = "db"
        };
    }

    private TMUser CreateUserData(CreateEmployeeCommand createEmployeeCommand)
    {
        string secretKey = _configuration.GetSection("AppSettings")["SECRET_KEY"];
        byte[] bytePass = $"{createEmployeeCommand.UserName.Trim().ToLower()}{secretKey}{createEmployeeCommand.Password}".ToMD5Password();
        TMUser userData = new TMUser
        {
            UserName = "admin",
            Password = bytePass,
            RoleID = 1,
            IsActive = true,
            ApproveStatus = 1
        };
        userData.SetCreatedDate();
        userData.SetCreatedBy();
        return userData;
    }
}
