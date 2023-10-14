using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace CYRetailIMS.Application.Services.PurchaseOrderService.Commands.CreatePurchaseOrder.v1;
public class CreateShipmentValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentValidator()
    {
		//TMShipmentType
		RuleFor(w => w.shipmenttypeid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุประเภทการส่งสินค้า");

		//TMWarehouse (ปัจจุบัน Fix = 1)
		RuleFor(w => w.warehouseid).NotNull().Must(w => w == 1).WithMessage("กรุณาระบุคลังสินค้าสำนักงานใหญ่เท่านั้น");
	}
}
