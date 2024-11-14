using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.ValidatePrintDraftItemTransferByDraftID.v1;
public class ValidatePrintDraftItemTransferValidator : AbstractValidator<ValidatePrintDraftItemTransferQuery>
{
    public ValidatePrintDraftItemTransferValidator()
    {
        RuleFor(x => x.draftid).NotNull().Must(x => x > 0).WithMessage("ข้อมูลการทำรายการไม่ถูกต้อง");
    }
}
