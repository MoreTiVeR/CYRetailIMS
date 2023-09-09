using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Queries.GetItemInBranchByBranchList.v1;

public class GetItemInBranchByBranchListValidator : AbstractValidator<GetItemInBranchByBranchListQuery>
{
	public GetItemInBranchByBranchListValidator()
	{
		//RuleFor(r => r.branchid_list).NotEmpty().NotNull().ChildRules(c => c.RuleFor(cr => !cr.TrueForAll(x => x >= 1))).WithMessage("Branch id must more than or equal 1");
		RuleFor(r => r.branchid_list)
			.Must(list => list != null && list.Count > 0 && list.All(branchId => branchId >= 1))
			.WithMessage("รหัสสาขาไม่ถูกต้อง");
		//RuleForEach(r => r.branchid_list).NotEmpty().NotNull().Must(s => s >= 1).WithMessage("Branch id must more than or equal 1");
	}
}
