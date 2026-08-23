
using FluentValidation;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v2;
public class CreateCountStockDetailValidator : AbstractValidator<CreateCountStockDetail>
{
    public CreateCountStockDetailValidator()
    {
        RuleFor(r => r.subitemtypeid).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลประเภทย่อยไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        //RuleFor(r => r.qtyinbranchofcountstockday).NotNull().Must(s => s >= 0).WithMessage("จำนวนสต๊อกหน้าร้าน ณ วันปัจจุบันไม่ถูกต้อง  กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.qtyinbranch).NotNull().Must(s => s >= 0).WithMessage("จำนวนสต๊อกหน้าร้านไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.countedamountqty).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลยอดนับไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.pendingrestockqty).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลรอเติมไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.damagedqty).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลชำรุดไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.salebeforecountqty).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลขายก่อนนับไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.totalcountqty).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลรวมนับไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.salebeforecountqty).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลขายก่อนนับไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        // No check for v2
        //RuleFor(r => r.shortagesurplusqty).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลขาดเกินไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }
}
