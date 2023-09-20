using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.BranchService.Commands.DeleteBranch.v1;
public class DeleteBranchValidator : AbstractValidator<DeleteBranchCommand>
{
    public DeleteBranchValidator()
    {
        RuleFor(r => r.branhid).NotNull().Must(m => m > 0).WithMessage("ข้อมูลสาขาที่ทำรายการไม่ถูกต้อง");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
