using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemBrandService.Queries.GetItemBrandByID.v1;
public class GetItemBrandByIDValidator : AbstractValidator<GetItemBrandByIDQuery>
{
    public GetItemBrandByIDValidator()
    {
        RuleFor(r => r.itembrandid).NotNull().Must(m => m > 0).WithMessage("Itembrandid must more than 0");
    }
}
