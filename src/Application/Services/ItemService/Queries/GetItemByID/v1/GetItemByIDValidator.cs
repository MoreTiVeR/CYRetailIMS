using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemService.Queries.GetItemByID.v1;
public class GetItemByIDValidator : AbstractValidator<GetItemByIDQuery>
{
    public GetItemByIDValidator()
    {
        RuleFor(r => r.itemid).NotNull().Must(m => m >= 1).WithMessage("Item id must more than or equal 1");
    }
}
