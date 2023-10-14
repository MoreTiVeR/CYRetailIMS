using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.PurchaseTypeService.Queries.PurchaseTypeByID.v1;
public class PurchaseTypeByIDValidator : AbstractValidator<PurchaseTypeByIDCommand>
{
	public PurchaseTypeByIDValidator()
	{
		RuleFor(w => w.purchasetypeid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุประเภทออเดอร์");
	}
}
