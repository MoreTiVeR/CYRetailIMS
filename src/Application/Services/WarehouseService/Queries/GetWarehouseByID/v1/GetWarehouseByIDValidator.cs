using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.WarehouseService.Queries.GetWarehouseByID.v1;
public class GetWarehouseByIDValidator : AbstractValidator<GetWarehouseByIDCommand>
{
	public GetWarehouseByIDValidator()
	{
		RuleFor(w => w.warehouseid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุคลังสินค้า");
	}
}
