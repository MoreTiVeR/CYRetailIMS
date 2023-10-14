using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeList.v1;
using FluentValidation;
using MediatR;

namespace CYRetailIMS.Application.Services.ShipmentTypeService.Queries.GetShipmentTypeByID.v1;
public class GetShipmentTypeByIDValidator : AbstractValidator<GetShipmentTypeByIDCommand>
{
    public GetShipmentTypeByIDValidator()
    {
		RuleFor(w => w.shipmenttypeid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุประเภทการขนส่ง");

	}

}
