using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.TransferTypeService.Queries.GetTransferTypeByID.v1;
public class GetTransferTypeByIDValidator : AbstractValidator<GetTransferTypeByIDQuery>
{
    public GetTransferTypeByIDValidator()
    {
        RuleFor(w => w.transfertypeid).NotNull().Must(x => x > 0);
    }
}
