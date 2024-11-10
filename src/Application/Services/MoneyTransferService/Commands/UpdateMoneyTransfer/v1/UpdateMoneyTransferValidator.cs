using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.MoneyTransferService.Commands.CreateMoneyTransfer.v1;
using FluentValidation;

namespace CYRetailIMS.Application.Services.MoneyTransferService.Commands.UpdateMoneyTransfer.v1;

public class UpdateMoneyTransferValidator : AbstractValidator<UpdateMoneyTransferCommand>
{
    public UpdateMoneyTransferValidator()
    {
        RuleFor(r => r.moneytransferid).NotNull().NotEmpty().WithMessage("กรุณาระบุหมายเลขโอน");
        RuleFor(r => r.transferdate).NotNull().WithMessage("กรุณาระบุวันโอน");
        RuleFor(r => r.amounttransfer).NotNull().NotEmpty().WithMessage("กรุณาระบุเงินโอน");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
