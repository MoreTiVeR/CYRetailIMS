using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.EmployeeService.Queries.GetEmployeeByID.v1;
public class GetEmployeeByIDValidator : AbstractValidator<GetEmployeeByIDQuery>
{
    public GetEmployeeByIDValidator()
    {
        RuleFor(w => w.empid).NotNull().Must(x => x > 0).WithMessage("ข้อมูลไอดีไม่ถูกต้อง");
    }
}
