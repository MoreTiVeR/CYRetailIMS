using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.AccountService.Queries.Login.v1;
public class LoginValidator : AbstractValidator<LoginQuery>
{
    public LoginValidator()
    {
        RuleFor(s => s.UserName).NotEmpty().WithMessage("ชื่อผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง");
        RuleFor(s => s.Password).NotEmpty().WithMessage("ชื่อผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง");
    }
}
