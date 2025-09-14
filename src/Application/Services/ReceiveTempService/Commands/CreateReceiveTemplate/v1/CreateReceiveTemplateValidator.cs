using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.CreateReceiveTemplate.v1;
public class CreateReceiveTemplateValidator : AbstractValidator<CreateReceiveTemplateCommand>
{
    public CreateReceiveTemplateValidator()
    {
        RuleFor(w => w.branchid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสาขา");
        RuleFor(w => w.shopheadernametext).NotEmpty().WithMessage("กรุณาระบุชื่อร้าน");
        RuleFor(w => w.shopheaderaddresstext).NotEmpty().WithMessage("กรุณาระบุที่อยู่ร้าน");
        RuleFor(w => w.telephoneno).NotEmpty().WithMessage("กรุณาระบุเบอร์โทรศัพท์สาขา");
        RuleFor(w => w.createdby).NotEmpty().WithMessage("กรุณาระบุผู้สร้าง");
        //RuleFor(w => w.createddate).NotNull().WithMessage("กรุณาระบุวันที่สร้าง");
    }
}
