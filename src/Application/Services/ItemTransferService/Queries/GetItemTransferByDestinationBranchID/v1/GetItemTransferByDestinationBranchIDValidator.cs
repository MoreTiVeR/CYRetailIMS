using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemTransferService.Queries.GetItemTransferByDestinationBranchID.v1;
public class GetItemTransferByDestinationBranchIDValidator : AbstractValidator<GetItemTransferByDestinationBranchIDQuery>
{
    public GetItemTransferByDestinationBranchIDValidator()
    {
        RuleFor(x => x.destinationbranchid).NotNull().Must(x => x > 0).WithMessage("ข้อมูลสาขาไม่ถูกต้อง");
    }
}
