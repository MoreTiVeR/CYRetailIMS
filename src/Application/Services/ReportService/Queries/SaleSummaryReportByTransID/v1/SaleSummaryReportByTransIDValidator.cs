using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleSummaryReportByTransID.v1;
public class SaleSummaryReportByTransIDValidator : AbstractValidator<SaleSummaryReportByTransIDQuery>
{

    public SaleSummaryReportByTransIDValidator()
    {
        RuleFor(w => w.transactionid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลประวัติที่ต้องการค้นหาไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");
    }
}
