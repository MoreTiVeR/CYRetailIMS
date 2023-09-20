using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.BranchService.Commands.CreateBranch.v1;
public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchValidator()
    {
        RuleFor(r => r.branchcode).NotNull().NotEmpty().WithMessage("กรุณาระบุรหัสสาขา");
        RuleFor(r => r.branchname).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อสาขา");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.creadeddate).NotNull().WithMessage("กรุณาระบุวันที่สร้าง");

        RuleFor(r => r.address).NotNull().NotEmpty().WithMessage("กรุณาระบุที่อยูสาขา");
    }
}
