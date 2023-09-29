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

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.DeleteEmployee.v1;
public class DeleteEmployeeHandler : BaseService, IRequestHandler<DeleteEmployeeCommand, BaseResponse<CommandResponse>>
{
    public DeleteEmployeeHandler(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<BaseResponse<CommandResponse>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        TMEmployee resEmp = await _unitOfWork.Repository<TMEmployee>().FirstOrDefaultAsync(w => w.EmpID == request.empid && w.IsActive);
        if(resEmp == null)
        {
            throw new Exception("ไม่พบข้อมูลพนักงาน");
        }

        resEmp.DeActiveStatus();
        resEmp.SetUpdatedDate(DateTime.Now);
        resEmp.SetUpdatedBy(request.updatedby);
        resEmp.AddDomainEvent(new TMEmployeeUpdateEvent(resEmp));
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
