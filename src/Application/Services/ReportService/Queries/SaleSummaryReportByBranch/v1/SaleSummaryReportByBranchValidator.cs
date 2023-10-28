using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByBranch.v1;
public class SaleSummaryReportByBranchValidator : AbstractValidator<SaleSummaryReportByBranchQuery>
{
    public SaleSummaryReportByBranchValidator()
    {
        RuleFor(w => w.branchid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลสาขาที่ทำรายการไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }
}
