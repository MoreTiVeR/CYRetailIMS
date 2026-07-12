using FluentValidation;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStockV2.v1;

public class CreateCountStockV2DetailValidator : AbstractValidator<CreateCountStockV2Detail>
{
    public CreateCountStockV2DetailValidator()
    {
        RuleFor(r => r.itemid)
            .Must(s => s > 0).WithMessage("รหัสสินค้าไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");

        RuleFor(r => r.qtyinbranchofcountstockday)
            .Must(s => s >= 0).WithMessage("จำนวนสต๊อกในระบบไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");

        RuleFor(r => r.physicalcountqty)
            .Must(s => s >= 0).WithMessage("จำนวนที่มีอยู่จริงไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");

        // shortagesurplusqty ไม่บังคับ >= 0 เพราะ V2 อนุญาตให้มีสต๊อกขาดได้
    }
}
