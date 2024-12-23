using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;

namespace CYRetailIMS.Application.Services.MoneyTransferSlipService.Queries.GetSlipByMoneyTransferID.v1;
public class GetSlipByMoneyTransferIDValidator : AbstractValidator<GetSlipByMoneyTransferIDQuery>
{
    public GetSlipByMoneyTransferIDValidator()
    {
        RuleFor(s => s.moneytransferid).NotNull().Must(s => s > 0).WithMessage("ข้อมูลการค้นหาไม่ถูกต้อง กระณาลองใหม่อีกครั้ง");
    }
}
