using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchID.v1;
public class GetItemInBranchByBranchIDValidator : AbstractValidator<GetItemInBranchByBranchIDQuery>
{
	public GetItemInBranchByBranchIDValidator()
	{
		RuleFor(r => r.branchid).Must(m => m >= 1).WithMessage("Branch id must more than or equal 1");
	}
}
