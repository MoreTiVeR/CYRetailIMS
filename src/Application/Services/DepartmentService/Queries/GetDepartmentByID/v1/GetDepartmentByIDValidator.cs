using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.DepartmentService.Queries.GetDepartmentByID.v1;
public class GetDepartmentByIDValidator : AbstractValidator<GetDepartmentByIDQuery>
{
    public GetDepartmentByIDValidator()
    {
        RuleFor(w => w.departmentid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุข้อมูลแผนก");
    }
}
