using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.UpdateReceiveTemplate.v1;
public class UpdateReceiveTemplateValidator : AbstractValidator<UpdateReceiveTemplateCommand>
{
    public UpdateReceiveTemplateValidator()
    {
        RuleFor(w => w.receivetemplateid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุรหัสแม่แบบใบรับสินค้า");
        RuleFor(w => w.branchid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสาขา");
        RuleFor(w => w.shopheadernametext).NotEmpty().WithMessage("กรุณาระบุชื่อร้าน");
        RuleFor(w => w.shopheaderaddresstext).NotEmpty().WithMessage("กรุณาระบุที่อยู่ร้าน");
        RuleFor(w => w.telephoneno).NotEmpty().WithMessage("กรุณาระบุเบอร์โทรศัพท์สาขา");
        RuleFor(w => w.updatedby).NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(w => w.isactive).NotNull().WithMessage("กรุณาระบุสถานะการใช้งาน");
    }
}
