using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.SupplierService.Commands.CreateSupplier.v1;
using FluentValidation;

namespace CYRetailIMS.Application.Services.SupplierService.Commands.UpdateSupplier.v1;
public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierValidator()
    {
        RuleFor(w => w.supplierid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุเลขซัฟพลายเออร์");
        RuleFor(w => w.suppliernameth).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อซัฟพลายเออร์ภาษาไทย");
        RuleFor(w => w.suppliernameth).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อซัฟพลายเออร์ภาษาอังกฤษ");
        RuleFor(w => w.suppliertypeid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุประเภทซัฟพลายเออร์");
        RuleFor(w => w.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleForEach(w => w.contact).SetValidator(new UpdateSupplierContactValidator());
    }
}

public class UpdateSupplierContactValidator : AbstractValidator<UpdateSupplierContact>
{
    public UpdateSupplierContactValidator()
    {
        RuleFor(w => w.suppliercontacttypeid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุประเภทการติดต่อซัฟพลายเออร์");
        RuleFor(w => w.contactaccountname).NotNull().NotEmpty().WithMessage("กรุณาระบุไอดีLine/Facebook/หรือชื่อ");
        RuleFor(w => w.contactperson).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อผู้ติดต่อ");
        RuleFor(w => w.mobileno).NotNull().NotEmpty().MinimumLength(10).MaximumLength(50).WithMessage("กรุณาระบุเบอร์ติดต่อให้ถูกต้อง");
    }
}
