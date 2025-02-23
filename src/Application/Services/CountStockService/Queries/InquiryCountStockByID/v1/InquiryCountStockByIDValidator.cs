using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByID.v1;
public class InquiryCountStockByIDValidator : AbstractValidator<InquiryCountStockByIDQuery>
{
    public InquiryCountStockByIDValidator()
    {
        RuleFor(s => s.countstockid).NotNull().Must(s => s > 0).WithMessage("ข้อมูลนับสต๊อกที่ทำรายการไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }
}
