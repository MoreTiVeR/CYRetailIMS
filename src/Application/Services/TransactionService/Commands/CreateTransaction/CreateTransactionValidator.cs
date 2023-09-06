using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(r => r.transactiontypeid).NotNull().Must(s => s > 0).WithMessage("ประเภทการขายปลีก-ส่ง");
        RuleFor(r => r.branchid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุสาขา");
        RuleFor(r => r.transactiondate).NotNull().WithMessage("กรุณาระบุวันที่ขายสินค้า");
        RuleFor(r => r.amounttransfer).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุยอดเงินโอน");
        RuleFor(r => r.amountdeposit).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุยอดเงินฝากธนาคาร");
        RuleFor(r => r.amountcash).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุยอดเงินคงเหลือหน้าร้าน");
        RuleFor(r => r.totalamount).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุยอดเงินขายสินค้าทั้งหมด");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.isactive).NotNull().Must(x => x == true || x == false).WithMessage("ระบุสถานะการทำรายการ เปิดใช้งาน|ไม่ใช้งาน");

        //RuleForEach(rr => rr.transactiondetail).Null().Empty().WithMessage("ข้อมูลสินค้าไม่ถูกต้อง กระณาตรวจสอบใหม่อีกครั้ง!").When(w => w.transactiondetail != null).SetValidator(new CreateTransactionDetailValidator());

        RuleFor(command => command.transactiondetail).NotEmpty().WithMessage("กรุณาระบุรายการขายสินค้าให้ถูกต้อง!");

        RuleForEach(r => r.transactiondetail).SetValidator(new CreateTransactionDetailValidator());

        //RuleForEach(r => r.transactiondetail).NotNull().NotEmpty().WithMessage("ข้อมูลสินค้าไม่ถูกต้อง กระณาตรวจสอบใหม่อีกครั้ง!")
        //    .SetValidator(new CreateTransactionDetailValidator()).When(w => w.transactiondetail != null);

        RuleFor(x => x.transactiondetail)
            //.Must(coll => coll.Sum(item => item.amount) == coll.Sum(item => item.price * item.qty)).WithMessage(x => $"จำนวนเงินรวม {x.transactiondetail.Sum(item => item.amount)} ไม่ตรงกับยอดขาย {x.transactiondetail.Sum(item => item.price * item.qty)} กรุณาตรวจสอบใหม่อีกครั้ง")
            .Must(coll => coll.Sum(item => item.amount) == coll.Sum(item => item.price * item.qty)).WithMessage(x => $"จำนวนเงินรวม ไม่ตรงกับยอดขายสินค้า กรุณาตรวจสอบใหม่อีกครั้ง")
            .Must(coll => coll.Sum(item => item.amount) > 0).WithMessage("จำนวนเงินรวมไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง")
            .When(x => x.transactiondetail != null);
    }
}
