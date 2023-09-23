using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByCriteria.v1;
public class GetItemInBranchByCriteriaValidator : AbstractValidator<GetItemInBranchByCriteriaQuery>
{

    public GetItemInBranchByCriteriaValidator()
    {
        RuleFor(w => w.branchid).NotNull().NotEmpty().WithMessage("ข้อมูลสาขาไม่ถูกต้อง");
        RuleFor(w => w.itemid).NotNull().NotEmpty().WithMessage("ข้อมูลสินค้าไม่ถูกต้อง");
    }
}
