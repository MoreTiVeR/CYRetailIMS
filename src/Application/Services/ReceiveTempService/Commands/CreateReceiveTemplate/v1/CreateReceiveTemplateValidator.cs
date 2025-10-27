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
        RuleFor(w => w.shopheadernametext)
            .NotEmpty().WithMessage("กรุณาระบุชื่อร้าน")
            .MaximumLength(50).WithMessage("ชื่อร้านความยาวไม่เกิน 50 ตัวอักษร");
        
        RuleFor(w => w.shopheaderaddresstext)
            .NotEmpty().WithMessage("กรุณาระบุที่อยู่ร้าน")
            .MaximumLength(200).WithMessage("ที่อยู่ร้านความยาวไม่เกิน 200 ตัวอักษร");
        
        RuleFor(w => w.shopfootertext)
            .MaximumLength(50).WithMessage("ข้อความท้ายกระดาษ(1) ความยาวไม่เกิน 50 ตัวอักษร");
        
        RuleFor(w => w.additionalfootertext)
            .MaximumLength(50).WithMessage("ข้อความท้ายกระดาษ(2) ความยาวไม่เกิน 50 ตัวอักษร");

        RuleFor(w => w.telephoneno)
            .NotEmpty().WithMessage("กรุณาระบุเบอร์โทรศัพท์สาขา")
            .MaximumLength(20).WithMessage("เบอร์โทรความยาวไม่เกิน 20 ตัวอักษร");

        RuleFor(w => w.printername)
            .NotEmpty().WithMessage("กรุณาระบุชื่อเครื่องพิมพ์")
            .MaximumLength(50).WithMessage("ชื่อเครื่องพิมพ์ความยาวไม่เกิน 50 ตัวอักษร");

        RuleFor(w => w.createdby).NotEmpty().WithMessage("กรุณาระบุผู้สร้าง");
        //RuleFor(w => w.createddate).NotNull().WithMessage("กรุณาระบุวันที่สร้าง");
    }
}
