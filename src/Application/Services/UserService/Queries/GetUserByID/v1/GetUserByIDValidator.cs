using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.UserService.Queries.GetUserByID.v1;
public class GetUserByIDValidator : AbstractValidator<GetUserByIDQuery>
{
    public GetUserByIDValidator()
    {
        RuleFor(w => w.userid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุไอดีสมาชิก");
    }
}
