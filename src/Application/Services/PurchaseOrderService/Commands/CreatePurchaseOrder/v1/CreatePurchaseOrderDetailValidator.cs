using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using FluentValidation;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
public class CreatePurchaseOrderDetailValidator : AbstractValidator<CreatePurchaseOrderDetailCommand>
{
    public CreatePurchaseOrderDetailValidator()
    {
        RuleFor(w => w.itemid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสินค้า");
        RuleFor(w => w.qty).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนสินค้า");
        RuleFor(w => w.price).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุราคาขาย");
        RuleFor(w => w.amount).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนเงินรวม");
        RuleFor(w => w.subtotal).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนเงินรวมทั้งหมด");
        RuleFor(w => w.total).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนรวมสุทธิ");
        RuleFor(r => r.total).Must((r, totalAmount) => IsValidTotalAmount(r, totalAmount)).WithMessage("ไม่สามารถทำรายการได้, เนื่องจากรายการสินค้ามียอดเงินรวมทั้งหมดไม่ตรงกัน");

    }

    private bool IsValidTotalAmount(CreatePurchaseOrderDetailCommand command, decimal total)
    {
        //If total less than 0
        if (command.total < 0)
        {
            return false;
        }

        // Calculate the price * qty = subtotal = total
        decimal subtotal = command.price * command.qty;

        // Compare the calculated total with the provided Total Amount
        return total == subtotal;
    }
}
