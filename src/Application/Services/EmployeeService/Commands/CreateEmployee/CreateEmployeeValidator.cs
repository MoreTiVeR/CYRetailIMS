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
        RuleFor(r => r.DepartmentID).NotEmpty().NotNull();
        RuleFor(r => r.FirstName).NotEmpty().NotNull();
        RuleFor(r => r.LastName).NotEmpty().NotNull();
        RuleFor(r => r.Email).NotEmpty().NotNull();
        RuleFor(r => r.Salary).NotEmpty().NotNull().Must(s => s > 0);
    }
}
