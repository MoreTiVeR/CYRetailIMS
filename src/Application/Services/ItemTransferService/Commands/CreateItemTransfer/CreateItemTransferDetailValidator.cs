using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer;
public class CreateItemTransferDetailValidator : AbstractValidator<CreateItemTransferDetailCommand>
{
    public CreateItemTransferDetailValidator()
    {
        RuleFor(r => r.itemid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุชนิดสินค้าให้ถูกต้อง");
        RuleFor(r => r.qty).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุจำนวนสินค้าให้ถูกต้อง");
    }
}
