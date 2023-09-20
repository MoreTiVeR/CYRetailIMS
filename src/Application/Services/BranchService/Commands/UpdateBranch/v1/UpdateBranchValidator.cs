using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.BranchService.Commands.DeleteBranch.v1;
using FluentValidation;

namespace CYRetailIMS.Application.Services.BranchService.Commands.UpdateBranch.v1;

public class UpdateBranchValidator : AbstractValidator<UpdateBranchCommand>
{
    public UpdateBranchValidator()
    {
        RuleFor(r => r.branhid).NotNull().Must(m => m > 0).WithMessage("ข้อมูลสาขาที่ทำรายการไม่ถูกต้อง");
        RuleFor(r => r.branchcode).NotNull().WithMessage("กรุณาระบุรหัสสาขา");
        RuleFor(r => r.branchname).NotNull().WithMessage("กรุณาระบุชื่อสาขา");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
