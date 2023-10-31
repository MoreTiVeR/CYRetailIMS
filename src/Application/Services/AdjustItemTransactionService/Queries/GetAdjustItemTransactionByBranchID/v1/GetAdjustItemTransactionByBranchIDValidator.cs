using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Queries.GetAdjustItemTransactionByBranchID.v1;
public class GetAdjustItemTransactionByBranchIDValidator : AbstractValidator<GetAdjustItemTransactionByBranchIDQuery>
{
    public GetAdjustItemTransactionByBranchIDValidator()
    {
        RuleFor(w => w.branchid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสาขาที่ต้องการค้นหา");
    }
}
