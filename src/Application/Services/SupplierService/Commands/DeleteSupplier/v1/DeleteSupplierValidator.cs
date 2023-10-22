using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.DeleteSupplier.v1;
public class DeleteSupplierValidator : AbstractValidator<DeleteSupplierCommand>
{
    public DeleteSupplierValidator()
    {
        RuleFor(w => w.supplierid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุเลขซัฟพลายเออร์");
        RuleFor(w => w.deleteddby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
    }
}
