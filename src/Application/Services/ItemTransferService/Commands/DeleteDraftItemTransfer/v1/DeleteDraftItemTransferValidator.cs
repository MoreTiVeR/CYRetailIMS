using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.DeleteDraftItemTransfer.v1;
public class DeleteDraftItemTransferValidator : AbstractValidator<DeleteDraftItemTransferCommand>
{
    public DeleteDraftItemTransferValidator()
    {
        RuleFor(w => w.draftid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลร่างโอนสินค้าไม่ถูกต้อง");
        RuleFor(w => w.updatedby).NotEmpty().WithMessage("ข้อมูลผู้ทำรายการไม่ถูกต้อง");
    }
}
