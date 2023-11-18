using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReportService.Queries.AvailableStockByBrachReport.v1;
public class AvailableStockByBrachReportValidator : AbstractValidator<AvailableStockByBrachReportQuery>
{
    public AvailableStockByBrachReportValidator()
    {
        RuleFor(w => w.branchid).NotNull().NotEmpty().Must(w => w > 0).WithMessage("กรุณาระบุสาขาให้ถูกต้อง");
    }
}
