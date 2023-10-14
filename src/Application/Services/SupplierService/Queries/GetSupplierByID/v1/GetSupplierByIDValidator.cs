using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.SupplierService.Queries.GetSupplierByID.v1;
public class GetSupplierByIDValidator : AbstractValidator<GetSupplierByIDCommand>
{
	public GetSupplierByIDValidator()
	{
		RuleFor(w => w.supplierid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุรหัสผู้ค้า");
	}
}
