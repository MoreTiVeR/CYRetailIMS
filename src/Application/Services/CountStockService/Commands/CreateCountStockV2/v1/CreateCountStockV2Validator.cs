using FluentValidation;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStockV2.v1;

public class CreateCountStockV2Validator : AbstractValidator<CreateCountStockV2Command>
{
    public CreateCountStockV2Validator()
    {
        RuleFor(s => s.branchid)
            .Must(s => s > 0).WithMessage("ข้อมูลสาขาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");

        RuleFor(s => s.countstockdate)
            .Must(s => s > DateTime.MinValue && s < DateTime.MaxValue)
            .WithMessage("วันที่นับสต๊อกไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");

        // V2: totalcount >= 0 (ไม่บังคับ > 0 เพราะสต๊อกอาจเป็น 0 ทั้งหมด)
        RuleFor(s => s.totalcount)
            .Must(s => s >= 0).WithMessage("จำนวนนับรวมไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");

        RuleFor(s => s.createdby)
            .NotNull().NotEmpty().WithMessage("ผู้ทำรายการไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");

        RuleFor(s => s.detail)
            .NotEmpty().WithMessage("รายการนับสินค้าไม่ถูกต้อง");

        RuleForEach(s => s.detail).SetValidator(new CreateCountStockV2DetailValidator());
    }
}
