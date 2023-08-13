using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.EmployeeService.Commands.CreateEmployee;
public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(r => r.departmentid).NotEmpty().NotNull();
        RuleFor(r => r.firstname).NotEmpty().NotNull();
        RuleFor(r => r.lastname).NotEmpty().NotNull();
        RuleFor(r => r.email).NotEmpty().NotNull();
        RuleFor(r => r.salary).NotEmpty().NotNull().Must(s => s > 0);
    }
}
