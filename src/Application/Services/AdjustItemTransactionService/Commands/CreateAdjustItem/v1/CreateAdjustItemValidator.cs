using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CYRetailIMS.Application.Services.TransactionService.Commands.CreateTransaction;
using FluentValidation;
using static CYRetailIMS.Application.Common.Models.EnumModel;

namespace CYRetailIMS.Application.Services.AdjustItemTransactionService.Commands.CreateAdjustItem.v1;
public class CreateAdjustItemValidator : AbstractValidator<CreateAdjustItemCommand>
{
    public CreateAdjustItemValidator()
    {
        //RuleFor(w => w.adjusttypeid).NotNull().Must(w => w == (int)AdjustItemType.Add || w == (int)AdjustItemType.Delete).WithMessage("ประเภทการปรับสต๊อกไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        //RuleFor(w => w.itemid).NotNull().Must(w => w > 0).WithMessage("สินค้าปรับสต็อกไม่ถูกต้อง กรุณาลองใม่อีกครั้ง");
        //RuleFor(w => w.branchid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลสาขาไม่ถูกต้อง กรุณาลองใม่อีกครั้ง");
        //RuleFor(w => w.qty).NotNull().Must(w => w > 0).WithMessage("จำนวนสินค้าปรับสต็อกไม่ถูกต้อง กรุณาลองใม่อีกครั้ง");
        RuleForEach(r => r.items).SetValidator(new CreateAdjustItemDetailValidator());
        RuleFor(r => r.createdby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.createddate).NotNull().WithMessage("กรุณาระบุวันที่สร้าง");
    }
}

public class CreateAdjustItemDetailValidator : AbstractValidator<CreateAdjustItemDetailCommand>
{
    public CreateAdjustItemDetailValidator()
    {
        RuleFor(w => w.adjusttypeid).NotNull().Must(w => w == (int)AdjustItemType.Add || w == (int)AdjustItemType.Delete).WithMessage("ประเภทการปรับสต๊อกไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(w => w.itemid).NotNull().Must(w => w > 0).WithMessage("สินค้าปรับสต็อกไม่ถูกต้อง กรุณาลองใม่อีกครั้ง");
        RuleFor(w => w.branchid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลสาขาไม่ถูกต้อง กรุณาลองใม่อีกครั้ง");
        RuleFor(w => w.qty).NotNull().Must(w => w > 0).WithMessage("จำนวนสินค้าปรับสต็อกไม่ถูกต้อง กรุณาลองใม่อีกครั้ง");
    }
}