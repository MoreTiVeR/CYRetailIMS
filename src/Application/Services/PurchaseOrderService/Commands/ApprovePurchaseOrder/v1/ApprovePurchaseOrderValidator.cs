using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.ApprovePurchaseOrder.v1;
public class ApprovePurchaseOrderValidator : AbstractValidator<ApprovePurchaseOrderCommand>
{
	public ApprovePurchaseOrderValidator()
	{
		RuleFor(w => w.purchaseorderid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุข้อมูลจัดซื้อ");
	}
}
