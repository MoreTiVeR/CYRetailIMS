using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
public class CreateCountStockDetailValidator : AbstractValidator<CreateCountStockDetail>
{
    public CreateCountStockDetailValidator()
    {
        RuleFor(r => r.subitemtypeid).NotNull().Must(s => s > 0).WithMessage("ข้อมูลประเภทย่อยไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.countedamountqty).NotNull().WithMessage("ข้อมูลยอดนับไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.pendingrestockqty).NotNull().WithMessage("ข้อมูลรอเติมไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.damagedqty).NotNull().WithMessage("ข้อมูลชำรุดไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.salebeforecountqty).NotNull().WithMessage("ข้อมูลขายก่อนนับไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.totalcountqty).NotNull().Must(s => s > 0).WithMessage("ข้อมูลรวมนับไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(r => r.salebeforecountqty).NotNull().WithMessage("ข้อมูลขาดเกินไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }
}
