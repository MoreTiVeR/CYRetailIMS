using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemTransferStatusService.Queries.GetItemTransferStatusByID.v1;
public class GetItemTransferStatusByIDValidator : AbstractValidator<GetItemTransferStatusByIDQuery>
{
    public GetItemTransferStatusByIDValidator()
    {
        RuleFor(w => w.transferstatusid).NotNull();
    }
}
