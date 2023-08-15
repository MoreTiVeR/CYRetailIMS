using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemTypeService.Queries.GetItemTypeByID.v1;
public class GetItemTypeByIDValidator : AbstractValidator<GetItemTypeByIDQuery>
{
    public GetItemTypeByIDValidator()
    {
        RuleFor(r => r.itemtypeid).NotNull().Must(m => m > 0).WithMessage("Itemtypeid must more than 0");
    }
}
