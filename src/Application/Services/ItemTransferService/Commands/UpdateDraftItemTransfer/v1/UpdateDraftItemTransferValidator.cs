using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer.v1;
using FluentValidation;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateDraftItemTransfer.v1;
public class UpdateDraftItemTransferValidator : AbstractValidator<UpdateDraftItemTransferCommand>
{
    public UpdateDraftItemTransferValidator()
    {
        RuleFor(w => w.draftid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลร่างโอนสินค้าไม่ถูกต้อง");
    }
}
