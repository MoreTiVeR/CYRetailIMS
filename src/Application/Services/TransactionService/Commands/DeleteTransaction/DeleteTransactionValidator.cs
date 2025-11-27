using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.DeleteTransaction;
public class DeleteTransactionValidator : AbstractValidator<DeleteTransactionCommand>
{
    public DeleteTransactionValidator()
    {
        RuleFor(r => r.transactionid).NotNull().Must(s => s > 0).WithMessage("ข้อมูลในการทำรายการไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!");
        RuleFor(r => r.deletedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.reason).NotNull().NotEmpty().WithMessage("กรุณาระบุสาเหตุที่ยกเลิกรายการ");
    }
}
