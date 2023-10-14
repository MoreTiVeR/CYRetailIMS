using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.CurrencyService.Queries.GetCurrencyByCode.v1;
public class GetCurrencyByCodeValidator : AbstractValidator<GetCurrencyByCodeCommand>
{
	public GetCurrencyByCodeValidator()
	{
		RuleFor(w => w.currencycode).NotNull().NotEmpty().WithMessage("กรุณาระสกุลเงิน");
	}
}
