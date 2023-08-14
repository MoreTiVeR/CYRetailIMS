using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.UnitOfMeasureService.Queries.GetUnitOfMeasureByID.v1;
public class GetUnitOfMeasureByIDValidator : AbstractValidator<GetUnitOfMeasureByIDQuery>
{
    public GetUnitOfMeasureByIDValidator()
    {
        RuleFor(r => r.unitofmeasureid).Must(s => s > 0).WithMessage("Unit of measure must more than or equal 1");
    }
}
