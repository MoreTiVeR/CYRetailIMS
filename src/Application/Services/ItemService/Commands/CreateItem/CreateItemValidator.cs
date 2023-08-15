
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemService.Commands.CreateItem;
public class CreateItemValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemValidator()
    {
        RuleFor(r => r.itemcode).NotNull().NotEmpty().WithMessage("กรุณาระบุรหัสสินค้า");
        RuleFor(r => r.itemtypeid).NotNull().Must(x => x > 0).WithMessage("กรุณาระบุประเภทสินค้า");
        RuleFor(r => r.unitofmeasureid).NotNull().Must(x => x > 0).WithMessage("กรุณาระบุหน่วยวัดสินค้า");
        RuleFor(r => r.brandid).NotNull().Must(x => x > 0).WithMessage("กรุณาระบุแบรนด์สินค้า");
        RuleFor(r => r.name).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อสินค้า");
        RuleFor(r => r.price).NotNull().Must(x => x > 0).WithMessage("ระบุราคาไม่ต่ำกว่า 0");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.isactive).NotNull().Must(x => x == true || x == false).WithMessage("ระบุสถานะสินค้า เปิดใช้งาน|ไม่ใช้งาน");
    }
}
