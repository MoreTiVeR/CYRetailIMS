using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
public class CreateSubItemTypeDetailValidator : AbstractValidator<CreateSubItemTypeDetail>
{
    public CreateSubItemTypeDetailValidator()
    {
        RuleFor(r => r.subitemcode).NotNull().NotEmpty().WithMessage("กรุณาระประเภทย่อย");
        RuleFor(r => r.subtypename_th).NotNull().WithMessage("กรุณาระบุชื่อประเภทย่อย");
        RuleFor(r => r.subTypename_en).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อประเภทย่อย");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
