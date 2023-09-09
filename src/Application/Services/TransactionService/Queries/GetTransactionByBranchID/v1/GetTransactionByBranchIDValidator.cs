using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.TransactionService.Queries.GetTransactionByBranchID.v1;
public class GetTransactionByBranchIDValidator : AbstractValidator<GetTransactionByBranchIDQuery>
{
	public GetTransactionByBranchIDValidator()
	{
		RuleFor(r => r.branchid).NotNull().NotEmpty().Must(w => w > 0).WithMessage("กรุณาระบุรหัสสาขาให้ถูกต้อง");
	}
}
