using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;

namespace CYRetailIMS.Application.Services.ItemService.Commands.UpdateItem;
public class UpdateItemValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemValidator()
    {
        RuleFor(r => r.itemid).NotNull().NotEmpty().WithMessage("กรุณาระบุไอดีสินค้า");
        RuleFor(r => r.brandid).NotNull().NotEmpty().WithMessage("กรุณาระบุแบรนด์สินค้า");
        RuleFor(r => r.name).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อสินค้า");
        RuleFor(r => r.price).NotNull().Must(x => x > 0).WithMessage("ระบุราคาห้ามต่ำกว่า 0");
        RuleFor(r => r.qty).NotNull().Must(x => x >= 0).WithMessage("ระบุจำนวนห้ามน้อยกว่า 0");
        RuleFor(r => r.notifyqty).NotNull().Must(x => x >= 0).WithMessage("ระบุจำนวนขั้นต่ำห้ามน้อยกว่า 0");
        RuleFor(r => r.discountpercent).NotNull().Must(x => x >= 0).WithMessage("ระบุส่วนลดห้ามต่ำกว่า 0%");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.isactive).NotNull().Must(x => x == true || x == false).WithMessage("ระบุสถานะสินค้า เปิดใช้งาน|ไม่ใช้งาน");
    }
}
