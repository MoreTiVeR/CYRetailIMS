using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.BranchService.Queries.GetBranchByID.v1;
public class GetBranchByIDValidator : AbstractValidator<GetBranchByIDQuery>
{
	public GetBranchByIDValidator()
	{
		RuleFor(r => r.branchid).NotNull().Must(m => m >= 1).WithMessage("Branch id must more than or equal 1");
	}
}
