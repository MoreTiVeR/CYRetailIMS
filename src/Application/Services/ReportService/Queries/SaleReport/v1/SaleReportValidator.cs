using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CYRetailIMS.Application.Services.ReportService.Queries.SaleReport.v1;
public class SaleReportValidator : AbstractValidator<SaleReportQuery>
{
	public SaleReportValidator()
	{
		//RuleFor(w => w.branchid).NotNull().Must(w => w > 0).WithMessage("ข้อมูลสาขาที่ทำรายการไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง");

	}
}
