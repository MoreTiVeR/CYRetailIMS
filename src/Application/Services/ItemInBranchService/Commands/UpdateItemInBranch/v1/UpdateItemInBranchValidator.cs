using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ItemInBranchService.Commands.UpdateItemInBranch.v1;
public class UpdateItemInBranchValidator : AbstractValidator<UpdateItemInBranchCommand>
{
    public UpdateItemInBranchValidator()
    {
        RuleFor(r => r.itemid).NotNull().NotEmpty().Must(x => x >= 1).WithMessage("กรุณาระบุสินค้าให้ถูกต้อง");
        RuleFor(r => r.branchid).NotNull().NotEmpty().Must(x => x >= 1).WithMessage("กรุณาระบุสาขาให้ถูกต้อง");
        //RuleFor(r => r.itemname).NotNull().NotEmpty().WithMessage("กรุณาระบุชื่อสินค้า");
        //RuleFor(r => r.itembrandid).NotNull().NotEmpty().Must(x => x >= 1).WithMessage("กรุณาระบุแบรนด์สินค้าให้ถูกต้อง");
        //RuleFor(r => r.itemtypeid).NotNull().NotEmpty().Must(x => x >= 1).WithMessage("กรุณาระบุประเภทสินค้าให้ถูกต้อง");
        //RuleFor(r => r.unitofmeasureid).NotNull().NotEmpty().Must(x => x >= 1).WithMessage("กรุณาระบุหน่วยนับสินค้าให้ถูกต้อง");
        RuleFor(r => r.qty).NotNull().NotEmpty().Must(x => x >= 0).WithMessage("กรุณาระบุจำนวนสินค้าให้ถูกต้อง");
        RuleFor(r => r.price).NotNull().NotEmpty().Must(x => x >= 1).WithMessage("กรุณาระบุราคาสินค้า");
        //RuleFor(r => r.cost).NotNull().NotEmpty().Must(x => x >= 0).WithMessage("กรุณาระบุต้นทุนสินค้าให้ถูกต้อง");
        //RuleFor(r => r.isactive).NotNull().Must(x => x == true || x == false).WithMessage("ระบุสถานะการทำรายการ เปิดใช้งาน|ไม่ใช้งาน");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.updateddate).NotNull().NotEmpty();
        
    }
}
