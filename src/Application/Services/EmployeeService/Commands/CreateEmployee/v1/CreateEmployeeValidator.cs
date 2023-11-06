using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee.v1;
public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        //RuleFor(r => r.username).NotEmpty().NotNull().WithMessage("กรุณาระบุชื่อผู้ใช้งาน");
        //RuleFor(r => r.username).MinimumLength(5).MaximumLength(20).WithMessage("ชื่อผู้ใช้งานมีความยาวขั้นต่ำ5ตัวอักษร แต่ไม่เกิน20ตัวอักษร");
        //RuleFor(r => r.password).NotEmpty().NotNull().WithMessage("กรุณาระบุรหัสผ่าน");
        //RuleFor(r => r.password).MinimumLength(5).MaximumLength(20).WithMessage("รหัสผ่านมีความยาวขั้นต่ำ8ตัวอักษร แต่ไม่เกิน20ตัวอักษร");
        //RuleFor(r => r.roleid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุสิทธิ์ในเข้าใช้งานระบบ");
        RuleFor(r => r.departmentid).NotEmpty().NotNull().WithMessage("กรุณาระบุแผนก");
        RuleFor(r => r.firstname).NotEmpty().NotNull().WithMessage("กรุณาระบุชื่อ");
        RuleFor(r => r.lastname).NotEmpty().NotNull().WithMessage("กรุณาระบุนามสกุล");
        RuleFor(r => r.email).NotEmpty().NotNull().WithMessage("กรุณาระบุอีเมล");
        //RuleFor(r => r.salary).NotEmpty().NotNull().Must(s => s >= 0).WithMessage("กรุณาระบุเงินเดือนพนักงาน");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.createddate).NotNull().Must(x => x > DateTime.MinValue && x <= DateTime.MaxValue).WithMessage("กรุณาระบุวันที่สร้างให้ถูกต้อง");
        //RuleFor(r => r.userinbranchid).NotEmpty().NotNull().Must(s => s > 0).WithMessage("กรุณาระบุสาขาผู้ใช้งาน");
    }
}
