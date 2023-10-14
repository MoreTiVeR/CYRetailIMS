using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
using FluentValidation;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderValidator()
    {
        RuleFor(r => r.purchasetypeid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุประเภทออร์เดอร์สินค้า");
        RuleFor(r => r.supplierid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุซัฟพลายเออร์");
        RuleFor(r => r.currencyid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสกุลเงินที่จ่ายค่าสินค้า เช่น THB");
        RuleFor(r => r.orderdate).NotNull().WithMessage("กรุณาระบุวันที่สั่งสินค้า");
        RuleFor(r => r.paymentypeid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุช่องทางการจ่ายเงิน");

        RuleFor(r => r.amount).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนเงินรวม");
        RuleFor(r => r.subtotal).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนเงินรวมสินค้า");
        RuleFor(r => r.total).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนเงินรวมทั้งหมด");

        RuleForEach(r => r.detail).SetValidator(new CreatePurchaseOrderDetailValidator());
        RuleFor(r => r.shipment).SetValidator(new CreateShipmentValidator());
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.createddate).NotNull().WithMessage("กรุณาระบุวันที่สร้าง");
    }
}


