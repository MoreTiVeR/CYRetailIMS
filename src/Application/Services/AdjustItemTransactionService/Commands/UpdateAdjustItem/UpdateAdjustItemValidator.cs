using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.UpdateAdjustItem;
public class UpdateAdjustItemValidator : AbstractValidator<UpdateAdjustItemCommand>
{
    public UpdateAdjustItemValidator()
    {
        RuleFor(w => w.adjustid).NotNull().WithMessage("ข้อมูลในการทำรายการไม่ถูกต้อง กระณาลองใหม่อีกครั้ง");
    }
}
