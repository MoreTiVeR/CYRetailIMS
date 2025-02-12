using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.CountStockService.Queries.InquiryCountStockByBranchID.v1;
public class InquiryCountStockByBranchIDValidator : AbstractValidator<InquiryCountStockByBranchIDQuery>
{
    public InquiryCountStockByBranchIDValidator()
    {
        RuleFor(s => s.branchid).NotNull().Must(s => s >= 0).WithMessage("ข้อมูลสาขาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }
}
