using FluentValidation;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.UpdateEmployee.v1;
public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(w => w.empid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุไอดีพนักงาน");
        RuleFor(w => w.departmentid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุแผนกพนักงาน");
        RuleFor(w => w.firstname).NotEmpty().NotNull().WithMessage("กรุณาระบุชื่อพนักงาน");
        RuleFor(w => w.lastname).NotEmpty().NotNull().WithMessage("กรุณาระบุนามกสุลพนักงาน");
        RuleFor(w => w.email).NotEmpty().NotNull().WithMessage("กรุณาระบุอีเมล");
        RuleFor(w => w.mobileno).NotEmpty().NotNull().MinimumLength(10).MaximumLength(10).WithMessage("กรุณาระบุเบอร์มือถือให้ถูกต้อง");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
