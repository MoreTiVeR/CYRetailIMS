using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByID.v1;
public class GetCurrencyByIDValidator : AbstractValidator<GetCurrencyByIDCommand>
{
	public GetCurrencyByIDValidator()
	{
		RuleFor(w => w.currencyid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุไอดีสกุลเงิน");
	}
}
