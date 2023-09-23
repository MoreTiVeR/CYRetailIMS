using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.DeleteItemInBranch.v1;
public class DeleteItemInBranchValidator : AbstractValidator<DeleteItemInBranchCommand>
{
    public DeleteItemInBranchValidator()
    {
        RuleFor(r => r.itemid).NotNull().NotEmpty().Must(x => x >= 1).WithMessage("กรุณาระบุสินค้าให้ถูกต้อง");
        RuleFor(r => r.branchid).NotNull().NotEmpty().Must(x => x >= 1).WithMessage("กรุณาระบุสาขาให้ถูกต้อง");
    }
}
