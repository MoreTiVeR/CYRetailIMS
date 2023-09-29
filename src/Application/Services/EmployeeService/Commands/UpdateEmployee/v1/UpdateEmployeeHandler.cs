using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Entities;
using CYRetailIMS.Domain.Events.TMEmployees;
using CYRetailIMS.Domain.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;
public class UpdateEmployeeHandler : BaseService, IRequestHandler<UpdateEmployeeCommand, BaseResponse<CommandResponse>>
{
    public UpdateEmployeeHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        TMEmployee resEmp = await _unitOfWork.Repository<TMEmployee>().FirstOrDefaultAsync(w => w.EmpID == request.empid);
        if (resEmp == null)
        {
            throw new Exception("ไม่พบข้อมูลพนักงาน");
        }

        #region Check dupplicate Name & LastName, Email
        var resExistEmp = await _unitOfWork.Repository<TMEmployee>().AnyAsync(w => w.Email == request.email
        || (w.FirstName.Trim() == request.firstname.Trim() && w.LastName.Trim() == request.lastname.Trim()));
        if(resExistEmp)
        {
			throw new Exception("มีชื่อ/นามสกลุลหรืออีเมล ซ้ำในระบบ");
		}
        #endregion

        #region Update properties
        resEmp.DepartmentID = request.departmentid;
        resEmp.FirstName = request.firstname;
        resEmp.LastName = request.lastname;
        if (!string.IsNullOrEmpty(request.nickname))
        {
            resEmp.NickName = request.nickname;
        }
        resEmp.Email = request.email;
        resEmp.MobileNo = request.mobileno;
        resEmp.IsActive = request.isactive;
        resEmp.SetUpdatedBy(request.updatedby);
        resEmp.SetUpdatedDate(request.updateddate);
        resEmp.AddDomainEvent(new TMEmployeeUpdateEvent(resEmp));
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
}
