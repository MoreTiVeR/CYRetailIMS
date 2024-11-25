using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CYRetailIMS.Application.Common.Models;
using CYRetailIMS.Domain.Infrastructure.Database;
using FluentValidation;
using MediatR;

namespace CYRetailIMS.Application.Services.ItemBrandService.Commands.UpdateBrand.v1;
public class UpdateBrandValidator : AbstractValidator<UpdateBrandCommand>
{

    public UpdateBrandValidator()
    {
        RuleFor(s => s.brandid).NotNull().Must(s => s > 0).WithMessage("ข้อมูลสาขาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
        RuleFor(s => s.brandname).NotEmpty().NotNull().WithMessage("กรุณาระบุชื่อแบรนด์");
        RuleFor(s => s.brandshortname).NotEmpty().NotNull().WithMessage("กรุณาระบุชื่อย่อ");
        RuleFor(r => r.updatedby).NotNull().NotEmpty().WithMessage("กรุณาระบุผู้ทำรายการ");
        RuleFor(r => r.isactive).NotNull().Must(x => x == true || x == false).WithMessage("ระบุสถานะการทำรายการ เปิดใช้งาน|ไม่ใช้งาน");
    }
}
