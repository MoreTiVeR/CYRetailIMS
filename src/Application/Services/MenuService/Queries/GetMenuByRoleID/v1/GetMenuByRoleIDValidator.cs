using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.MenuService.Queries.GetMenuByRoleID.v1;
public class GetMenuByRoleIDValidator : AbstractValidator<GetMenuByRoleIDQuery>
{
    public GetMenuByRoleIDValidator()
    {
        RuleFor(r => r.roleid).NotNull().Must(m => m > 0).WithMessage("Invalid requst");
    }
}
