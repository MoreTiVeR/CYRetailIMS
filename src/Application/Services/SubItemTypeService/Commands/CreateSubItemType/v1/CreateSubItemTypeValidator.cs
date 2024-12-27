using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.SubItemTypeService.Commands.CreateSubItemType.v1;
public class CreateSubItemTypeValidator : AbstractValidator<CreateSubItemTypeCommand>
{
    public CreateSubItemTypeValidator()
    {
        RuleForEach(s => s.subitemtypelist).SetValidator(new CreateSubItemTypeDetailValidator());
    }
}
