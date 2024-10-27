using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.CreateItemTransfer.v1;
public class CreateItemTransferValidator : AbstractValidator<CreateItemTransferCommand>
{
    public CreateItemTransferValidator()
    {
        RuleFor(r => r.transfertypeid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุประเภทการโอนสินค้า");
        RuleFor(r => r.sourceid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุคลัง/สาขาต้นทาง");
        RuleFor(r => r.destinationid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุคลัง/สาขาปลายทาง");
        RuleFor(r => r.destinationid).NotNull().Must(s => s > 0).WithMessage("กรุณาระบุคลัง/สาขาปลายทาง");
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.isactive).NotNull().Must(x => x == true || x == false).WithMessage("ระบุสถานะการทำรายการ เปิดใช้งาน|ไม่ใช้งาน");
        RuleFor(r => r.transferstatus).NotNull().Must(s => s == (int)ApproveStatus.WaitingApprove).WithMessage("ระบุสถานะการโอนเป็นรออนุมัติเท่านั้น");
        RuleFor(command => command.items).NotEmpty().WithMessage("กรุณาระบุรายการโอนสินค้าให้ถูกต้อง!");
        RuleForEach(x => x.items).SetValidator(new CreateItemTransferDetailValidator());
    }
}
