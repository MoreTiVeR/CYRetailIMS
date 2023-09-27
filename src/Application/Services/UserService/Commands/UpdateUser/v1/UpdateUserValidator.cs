using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.UserService.Commands.UpdateUser.v1;
public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(r => r.userid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุไอดีให้ถูกต้อง");
        RuleFor(r => r.roleid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุสิทธิ์ในเข้าใช้งานระบบ");
        RuleFor(r => r.userinbranchid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุสาขาผู้ใช้งาน");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.updateddate).NotNull().WithMessage("กรุณาระบุวันที่ทำรายการ");
    }
}
