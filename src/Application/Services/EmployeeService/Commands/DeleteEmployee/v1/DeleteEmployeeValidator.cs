using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.DeleteEmployee.v1;
public class DeleteEmployeeValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeValidator()
    {
        RuleFor(w => w.empid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลไม่ถูกต้อง");
        RuleFor(w => w.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
