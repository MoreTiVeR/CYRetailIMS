using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.SupplierContactTypeService.Queries.GetSupplierContactTypeByID.v1;
public class GetSupplierContactTypeByIDValidator : AbstractValidator<GetSupplierContactTypeByIDCommand>
{
	public GetSupplierContactTypeByIDValidator()
	{
		RuleFor(w => w.suppliercontacttypeid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุประเภทผู้จัดซื้อ");
	}
}
