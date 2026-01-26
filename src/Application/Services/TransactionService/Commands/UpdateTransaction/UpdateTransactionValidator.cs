using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.UpdateTransaction;
public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator()
    {
        RuleFor(r => r.transactionid).NotNull().Must(s => s > 0).WithMessage("ข้อมูลในการทำรายการไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!");
        // transactiondate must be provided and be today or a future date
        RuleFor(r => r.transactiondate)
            .NotNull().WithMessage("กรุณาระบุวันที่ขายสินค้า")
            .Must(d => d.Date >= DateTime.Today).WithMessage("ไม่สามารถแก้ไขรายการขายย้อนหลังได้");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
