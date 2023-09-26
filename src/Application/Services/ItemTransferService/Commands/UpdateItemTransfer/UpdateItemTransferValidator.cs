using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using FluentValidation;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.ItemTransferService.Commands.UpdateItemTransfer;
public class UpdateItemTransferValidator : AbstractValidator<UpdateItemTransferCommand>
{
    public UpdateItemTransferValidator()
    {
        RuleFor(w => w.transferid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลรายการโอนไม่ถูกต้อง");
        RuleFor(w => w.sourceid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสาขาต้นทาง");
        RuleFor(w => w.destinationid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสาขาปลายทาง");
        RuleFor(w => w.itemid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสินค้า");
        RuleFor(w => w.qty).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนสินค้า");
        //RuleFor(w => w.receiveqty).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุจำนวนสินค้าที่รับเข้า");
        //RuleFor(w => w.returnqty).NotNull().Must(w => w > 0);
        RuleFor(w => w.description).NotNull().NotEmpty().WithMessage("กรุณาระบุหมายเหตุ");
        RuleFor(w => w.transferstatusid).NotNull().Must(w => w > 0).WithMessage("กรุณาระบุสถานะการโอนสินค้า");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.updateddate).NotNull().Must(x => x > DateTime.MinValue && x <= DateTime.MaxValue).WithMessage("กรุณาระบุวันที่สร้างให้ถูกต้อง");
        RuleFor(r => r.qty).Must((r, qty) => IsValidTotalQTY(r, qty)).WithMessage("จำนวนรวมรับ/คืนสินค้า ไม่ตรงกับจำนวนโอน, กรุณาตรวจสอบใหม่อีกครั้ง");
    }

    private bool IsValidTotalQTY(UpdateItemTransferCommand command, int qty)
    {
        if(command.transferstatusid == (int)TransferStatus.Received)
        {
            // Calculate the sum of receive and return qty
            int receivelQTY = command.receiveqty + command.returnqty;

            // Compare the calculated receivelQTY with the transfer qty
            return qty == receivelQTY;
        }
        else
        {
            return true;
        }

    }
}
