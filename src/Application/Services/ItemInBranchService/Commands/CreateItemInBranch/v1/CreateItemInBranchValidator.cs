using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ItemService.Commands.CreateItemList;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.CreateItemInBranch.v1;

public class CreateItemInBranchValidator : AbstractValidator<CreateItemInBranchListCommand>
{
    public CreateItemInBranchValidator()
    {
        RuleForEach(r => r.items).SetValidator(new CreateItemInBranchDetailValidator());
    }
}

public class CreateItemInBranchDetailValidator : AbstractValidator<CreateItemInBranchDetailCommand>
{
    public CreateItemInBranchDetailValidator()
    {
        RuleFor(r => r.itemid).NotNull().Must(x => x > 0).WithMessage("กรุณาระบุไอดีสินค้า");
        RuleFor(r => r.itemcode).NotNull().NotEmpty().WithMessage("กรุณาระบุรหัสสินค้า");
        RuleFor(r => r.itemtypeid).NotNull().Must(x => x > 0).WithMessage("กรุณาระบุประเภทสินค้า");
        RuleFor(r => r.unitofmeasureid).NotNull().Must(x => x > 0).WithMessage("กรุณาระบุหน่วยวัดสินค้า");
        RuleFor(r => r.brandid).NotNull().Must(x => x > 0).WithMessage("กรุณาระบุแบรนด์สินค้า");
        RuleFor(r => r.name).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อสินค้า");
        RuleFor(r => r.price).NotNull().Must(x => x > 0).WithMessage("ระบุราคาห้ามต่ำกว่า 0");
        RuleFor(r => r.qty).NotNull().Must(x => x >= 0).WithMessage("ระบุจำนวนห้ามน้อยกว่า 0");
        RuleFor(r => r.discountpercent).NotNull().Must(x => x >= 0).WithMessage("ระบุส่วนลดห้ามต่ำกว่า 0%");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.isactive).NotNull().Must(x => x == true || x == false).WithMessage("ระบุสถานะสินค้า เปิดใช้งาน|ไม่ใช้งาน");
    }
}
