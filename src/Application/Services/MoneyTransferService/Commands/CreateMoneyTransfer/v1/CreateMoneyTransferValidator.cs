using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
public class CreateMoneyTransferValidator : AbstractValidator<CreateMoneyTransferCommand>
{
    public CreateMoneyTransferValidator()
    {
        RuleFor(r => r.branchid).NotNull().NotEmpty().WithMessage("กรุณาระบุรหัสสาขา");
        RuleFor(r => r.transferdate).NotNull().WithMessage("กรุณาระบุวันโอน");
        RuleFor(r => r.amounttransfer).NotNull().NotEmpty().WithMessage("กรุณาระบุเงินโอน");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
