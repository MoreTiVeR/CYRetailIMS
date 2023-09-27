using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.RoleService.Queries.GetRoleByID.v1;
public class GetRoleByIDValidator : AbstractValidator<GetRoleByIDQuery>
{
    public GetRoleByIDValidator()
    {
        RuleFor(w => w.roleid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุข้อมูลสิทธิ์");
    }
}
