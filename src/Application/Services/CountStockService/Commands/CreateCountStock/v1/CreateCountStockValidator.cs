using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.CountStockService.Commands.CreateCountStock.v1;
public class CreateCountStockValidator : AbstractValidator<CreateCountStockCommand>
{

    public CreateCountStockValidator()
    {
        RuleFor(s => s.branchid).NotNull().Must(s => s > 0).WithMessage("ข้อมูลสาขาไม่ถุกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(s => s.countstockdate).NotNull().Must(s => s > DateTime.MinValue && s < DateTime.MaxValue).WithMessage("วันที่นับสตีอกไม่ถุกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(s => s.totalcount).NotNull().Must(s => s > 0).WithMessage("จำนวนนับรวมไม่ถุกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(s => s.createdby).NotNull().NotEmpty().WithMessage("ผู้ทำรายการไม่ถุกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(s => s.detail).NotEmpty().WithMessage("รายการนับสินค้าไม่ถูกต้อง");
        RuleForEach(s => s.detail).SetValidator(new CreateCountStockDetailValidator());
    }
}
