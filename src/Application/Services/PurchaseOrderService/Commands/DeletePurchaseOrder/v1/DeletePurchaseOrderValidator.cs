using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.DeletePurchaseOrder.v1;
internal class DeletePurchaseOrderValidator : AbstractValidator<DeletePurchaseOrderCommand>
{
	public DeletePurchaseOrderValidator()
	{
		RuleFor(w => w.purchaseorderid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุข้อมูลจัดซื้อ");
	}
}
