using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.UserService.Commands.CreateUser.v1;
public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
	public CreateUserValidator()
	{
		RuleFor(r => r.empid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุพนักงานที่ลงทะเบียนใช้งานระบบ");
		//RuleFor(r => r.username).NotEmpty().NotNull().WithMessage("กรุณาระบุชื่อผู้ใช้งาน");
		//RuleFor(r => r.username).MinimumLength(5).MaximumLength(20).WithMessage("ชื่อผู้ใช้งานมีความยาวขั้นต่ำ5ตัวอักษร แต่ไม่เกิน20ตัวอักษร");
        RuleFor(r => r.username)
			.Length(5, 20).When(user => !string.IsNullOrEmpty(user.username)).WithMessage("ชื่อผู้ใช้งานมีความยาวขั้นต่ำ5ตัวอักษร แต่ไม่เกิน20ตัวอักษร");
  //      RuleFor(r => r.password).NotEmpty().NotNull().WithMessage("กรุณาระบุรหัสผ่าน");
		//RuleFor(r => r.password).MinimumLength(5).MaximumLength(20).WithMessage("รหัสผ่านมีความยาวขั้นต่ำ8ตัวอักษร แต่ไม่เกิน20ตัวอักษร");
        RuleFor(r => r.password)
            .Length(5, 20).When(user => !string.IsNullOrEmpty(user.password)).WithMessage("รหัสผ่านมีความยาวขั้นต่ำ5ตัวอักษร แต่ไม่เกิน20ตัวอักษร");
        RuleFor(r => r.roleid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุสิทธิ์ในเข้าใช้งานระบบ");
		RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
		RuleFor(r => r.createddate).NotNull().WithMessage("กรุณาระบุวันที่สร้าง");
		RuleFor(r => r.userinbranchid).NotEmpty().NotNull().Must(s => s > 0).WithMessage("กรุณาระบุสาขาผู้ใช้งาน");
	}
}
