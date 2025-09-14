using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReceiveTempService.Commands.DeleteReceiveTemplate.v1;
public class DeleteReceiveTemplateValidator : AbstractValidator<DeleteReceiveTemplateCommand>
{
    public DeleteReceiveTemplateValidator()
    {
        RuleFor(w => w.receivetemplateid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุรหัสแม่แบบใบรับสินค้า");
        RuleFor(w => w.updatedby).NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
