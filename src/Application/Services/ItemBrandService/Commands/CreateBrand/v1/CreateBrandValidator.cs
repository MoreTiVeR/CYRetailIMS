using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemBrandService.Commands.CreateBrand.v1;
public class CreateBrandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandValidator()
    {
        RuleFor(r => r.brandname).NotEmpty().WithMessage("กรุณาระบุชื่อแบรนด์");
        RuleFor(r => r.brandshortname).NotEmpty().WithMessage("กรุณาระบุชื่อย่อแบรนด์");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.creadeddate).NotNull().WithMessage("กรุณาระบุวันที่สร้าง");
    }
}
