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
        RuleFor(r => r.transactiondate).NotNull().WithMessage("กรุณาระบุวันที่ขายสินค้า");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
