using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
using FluentValidation;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.UpdatePurchaseOrder.v1;
public class UpdatePurchaseOrderValidator : AbstractValidator<UpdatePurchaseOrderCommand>
{
	public UpdatePurchaseOrderValidator()
	{
		RuleFor(w => w.purchaseorderid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุข้อมูลจัดซื้อ");
        RuleForEach(r => r.detail).SetValidator(new CreatePurchaseOrderDetailValidator());
    }
}
