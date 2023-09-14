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
        TMUsers isExistUser = (from a in await _unitOfWork.Repository<TMUsers>().QueryAsync()
                              join b in await _unitOfWork.Repository<TMEmployee>().QueryAsync() on a.UserID equals b.UserID
                              where a.UserName == request.username || b.Email == request.email
                              select a).FirstOrDefault();

        if (isExistUser != null)
        {
            throw new Exception("มีชื่อผู้ใช้งานนี้ในระบบแล้ว กรุณาลองใหม่อีกครั้ง");
        }

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
            result = true,
            data = new CommandResponse { result = true },
            status = StatusCodes.Status200OK.ToString(),
            message = "Success",
            soruce = "db"
        };
    }

    private TMUsers CreateUserData(CreateEmployeeCommand createEmployeeCommand)
    {
        string secretKey = _configuration.GetSection("AppSettings")["SECRET_KEY"];
        byte[] bytePass = $"{createEmployeeCommand.username.Trim().ToLower()}{secretKey}{createEmployeeCommand.password}".ToMD5Password();
        TMUsers userData = new TMUsers
        {
            UserName = createEmployeeCommand.username,
            Password = bytePass,
            RoleID = createEmployeeCommand.roleid,
            IsActive = true,
            ApproveStatus = 0
        };
        userData.SetCreatedDate();
        userData.SetCreatedBy();
        return userData;
    }
}
