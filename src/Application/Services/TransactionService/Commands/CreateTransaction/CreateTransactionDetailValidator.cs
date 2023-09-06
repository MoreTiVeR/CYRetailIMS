using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
public class CreateTransactionDetailValidator : AbstractValidator<CreateTransactionDetailCommand>
{
    public CreateTransactionDetailValidator()
    {
        RuleFor(r => r.itemid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุชนิดสินค้าให้ถูกต้อง");
        RuleFor(r => r.price).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุราคาสินค้าให้ถูกต้อง");
        RuleFor(r => r.qty).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุจำนวนสินค้าให้ถูกต้อง");
        RuleFor(r => r.amount).NotNull().Must(s => s > 0).WithMessage("จำนวนเงินขายรวมตามชนิดสินค้าไม่ถูกต้อง");
        RuleFor(r => r.isactive).NotNull().Must(x => x == true || x == false).WithMessage("ระบุสถานะการทำรายการ เปิดใช้งาน|ไม่ใช้งาน");
    }
}
