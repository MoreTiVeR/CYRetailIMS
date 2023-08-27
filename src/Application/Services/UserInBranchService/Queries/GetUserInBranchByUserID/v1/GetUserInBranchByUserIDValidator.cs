using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.UserInBranchService.Queries.GetUserInBranchByUserID.v1;
public class GetUserInBranchByUserIDValidator : AbstractValidator<GetUserInBranchByUserIDQuery>
{
	public GetUserInBranchByUserIDValidator()
	{
		RuleFor(r => r.userid).Must(m => m >= 1).WithMessage("User id is required");
	}
}
