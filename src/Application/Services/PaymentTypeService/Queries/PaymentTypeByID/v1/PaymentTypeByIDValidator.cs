using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.PaymentTypeService.Queries.PaymentTypeByID.v1;
public class PaymentTypeByIDValidator : AbstractValidator<PaymentTypeByIDCommand>
{
	public PaymentTypeByIDValidator()
	{
		RuleFor(w => w.paymentid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุรหัสการจ่ายเงิน");
	}
}
